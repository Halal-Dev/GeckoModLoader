using System;
using System.IO;
using System.Xml.Serialization;

namespace GeckoMapTester
{
    [Serializable]
    public class Configuration
    {
        public String lastIp;

        private static XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
        public static Configuration currentConfig;
    
        public static void Load()
        {
            if (!File.Exists("TesterConfig.xml"))
            {
                currentConfig = new Configuration();
                currentConfig.lastIp = "192.168.1.1";
                Save();
            }
            else
            {
                using (FileStream stream = File.OpenRead("TesterConfig.xml"))
                {
                    currentConfig = (Configuration)serializer.Deserialize(stream);
                }
            }
        }

        public static void Save()
        {
            File.Delete("TesterConfig.xml");
            using (FileStream writer = File.OpenWrite("TesterConfig.xml"))
            {
                serializer.Serialize(writer, currentConfig);
            }
        }

    }
}
