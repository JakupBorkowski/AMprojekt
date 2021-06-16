using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MultiViewApp.Model
{
    public class ConfigParams
    {
        static readonly string defaultIpAdress = "192.168.56.15";
        static readonly string defaultIpPort = "8080";
        static readonly string defaultApiVersion = "1.0.0";
        public static readonly byte defaultAlphaChannge = 153;
        public string ApiVersion;
        public string IpAddress;
        public string IpPort;
        static readonly int defaultSampleTime = 500;
        public int SampleTime;
        public readonly int defaultMaxSamples = 100;
        public int MaxSamples;

        public double XAxisMax
        {
            get
            {
                return defaultMaxSamples * SampleTime / 1000.0;
            }
            private set { }
        }

        public ConfigParams()
        {
            if (File.Exists("jsonData.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader("jsonData.json"))
                    {
                        try
                        {
                            dynamic configJson = JObject.Parse(sr.ReadToEnd());
                            IpAddress = configJson.IpAddress;
                            IpPort = configJson.IpPort;
                            SampleTime = configJson.SampleTime;
                            MaxSamples = configJson.MaxSamples;
                            ApiVersion = configJson.ApiVersion;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Json Object Error");
                            Debug.WriteLine(e);
                        }

                    }
                }
                catch (IOException e)
                {
                    Debug.WriteLine("Cannot open config file.");
                    Debug.WriteLine(e);
                }

            }
            else
            {
                this.SetDefaultConfig();
            }


        }

        public ConfigParams(string _ip, string _ipport, string _api, int _ms, int _st)
        {
            IpAddress = _ip;
            IpPort = _ipport;
            SampleTime = _st;
            MaxSamples = _ms;
            ApiVersion = _api;
        }

        public ConfigParams(string ip, int st)
        {
            IpAddress = ip;
            SampleTime = st;
        }

        private JObject GetJsonObject()
        {
            JObject jsonObj = new JObject(
                new JProperty("IpAddress", IpAddress),
                new JProperty("IpPort", IpPort),
                new JProperty("SampleTime", SampleTime),
                new JProperty("MaxSamples", MaxSamples),
                new JProperty("ApiVersion", ApiVersion));

            return jsonObj;

        }
        public void SaveConfigToFile()
        {
            string output = JsonConvert.SerializeObject(GetJsonObject());

            try
            {
                using (StreamWriter sw = new StreamWriter("jsonData.json"))
                {
                    sw.WriteLine(output);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("Cannot save to config file.");
                Debug.WriteLine(e);
            }
        }

        public void SetDefaultConfig()
        {
            IpAddress = defaultIpAdress;
            IpPort = defaultIpPort;
            SampleTime = defaultSampleTime;
            MaxSamples = defaultMaxSamples;
            ApiVersion = defaultApiVersion;
        }
    }
}
