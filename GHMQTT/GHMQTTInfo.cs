using System;
using System.Drawing;

using Grasshopper.Kernel;

namespace GHMQTT
{
    public class GHMQTTInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHMQTT Info";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                return "A grasshopper component that can connect to a MQTT broker.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("0c1f13db-f869-4652-b3c1-49e3a4c147dd");
            }
        }

        public override string AuthorName
        {
            get
            {
                return "Joël Gähwiler";
            }
        }
        public override string AuthorContact
        {
            get
            {
                return "joel.gaehwiler@gmail.com";
            }
        }
    }
}
