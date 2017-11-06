using System;

using Grasshopper.Kernel;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GHMQTT
{
    public class GHMQTTComponent : GH_Component
    {
        private MqttClient client;

        private double value = 0;

        public GHMQTTComponent() : base("MQTT", "MQTT", "Receive numbers from an MQTT topic.", "Network", "") {}

        public override void AddedToDocument(GH_Document document)
        {
            // TODO: Make host and port configurable.

            // create client instance
            client = new MqttClient("localhost", 1884, false, null, null, MqttSslProtocols.None);

            // register callback
            client.MqttMsgPublishReceived += MessageReceived;

            // connect to broker
            client.Connect("");

            // subscribe to the topic
            client.Subscribe(new string[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

            base.AddedToDocument(document);
        }

        void MessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // TODO: Parse comma seperated values and return array?

            // convert bytes to text
            string val = System.Text.Encoding.UTF8.GetString(e.Message);

            // TODO: Synchronize value access.

            // get value
            value = Convert.ToDouble(val);

            // TODO: Run in main thread.

            // expire solution
            ExpireSolution(true);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // add input parameters
            // pManager.AddTextParameter("Broker URI", "B", "Base plane for spiral", GH_ParamAccess.item, "mqtt://0.0.0.0.0:1883");
            // pManager.AddTextParameter("MQTT Topic", "T", "The topic to receive values from.", GH_ParamAccess.item, "value");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // add output parameters
            pManager.AddNumberParameter("Value", "V", "The value", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //// prepare variables
            //String brokerURI = "";
            //String topic = "";

            //// get data
            //if (!DA.GetData(0, ref brokerURI)) return;
            //if (!DA.GetData(1, ref topic)) return;

            //// validate broker url
            //if (brokerURI == "")
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing Nroker URI");
            //    return;
            //}

            //// validate topic
            //if (topic == "")
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing Topic");
            //    return;
            //}

            // set output value
            DA.SetData(0, value);
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            // deregister callback
            client.MqttMsgPublishReceived -= MessageReceived;

            // disconenct client if existing
            client.Disconnect();

            base.RemovedFromDocument(document);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("16dbbb54-ddf0-42dd-8bbf-ba0b2fee3157"); }
        }
    }
}
