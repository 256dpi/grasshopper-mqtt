using System;

using Grasshopper.Kernel;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GHMQTT
{
    public class GHMQTTComponent : GH_Component
    {
        private string lastURI = "";
        private MqttClient client;

        private Object mutex = new Object();
        private string topic = "";
        private string payload = "";

        public GHMQTTComponent() : base("MQTT", "MQTT", "Receive data from an MQTT topic.", "Network", "MQTT") {}

        public override void AddedToDocument(GH_Document document)
        {
            base.AddedToDocument(document);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // add input parameters
            pManager.AddTextParameter("Broker URI", "B", "The URI of the broker", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // add output parameters
            pManager.AddTextParameter("Topic", "T", "The topic", GH_ParamAccess.item);
            pManager.AddTextParameter("Payload", "P", "The payload", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // set output value
            lock (mutex)
            {
                DA.SetData(0, topic);
                DA.SetData(1, payload);
            }

            // get data
            String rawURI = "";
            if (!DA.GetData(0, ref rawURI)) return;

            // return if uri has not changed
            if (lastURI == rawURI)
            {
                return;
            }

            // set last uri
            lastURI = rawURI;

            // otherwise prepare for connection change

            // validate uri
            if (rawURI == "")
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing URI");
                return;
            }

            // parse uri
            var uri = new Uri(rawURI);

            // disconnect current client (ignoring errors)
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                } catch {}

                client = null;
            }

            // create client instance
            client = new MqttClient(uri.Host, uri.Port, false, null, null, MqttSslProtocols.None);

            // register callback
            client.MqttMsgPublishReceived += MessageReceived;

            // get username password
            var creds = uri.UserInfo.Split(':');

            // connect to broker
            if (creds.Length == 0) {
                client.Connect("");    
            } else if (creds.Length == 1) {
                client.Connect("", creds[0], "");    
            } else if(creds.Length == 2) {
                client.Connect("", creds[0], creds[1]);    
            }

            // default to # if path is empty
            var path = uri.AbsolutePath;
            if (path == "") {
                path = "#";
            }

            // subscribe to the topic
            client.Subscribe(new string[] { path }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }

        void MessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // update topic and payload
            lock (mutex)
            {
                topic = e.Topic;
                payload = System.Text.Encoding.UTF8.GetString(e.Message);
            }

            // expire solution
            // TODO: Run in main thread.
            //ExpireSolution(true);
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            // disconenct client (ignoring errors)
            if (client != null) {
                try {
                    client.Disconnect();    
                } catch {}
            }

            base.RemovedFromDocument(document);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("16dbbb54-ddf0-42dd-8bbf-ba0b2fee3157"); }
        }
    }
}
