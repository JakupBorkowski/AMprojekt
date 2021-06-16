#define CLIENT
#define GET
//#define DYNAMIC

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Echovoice.JSON;
using MultiViewApp.Model;
namespace MultiViewApp.ViewModel
{
    
    class View2_ViewModel : BaseViewModel
    {
        #region Properties
        private string ipAddress;
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                if (ipAddress != value)
                {
                    ipAddress = value;
                    OnPropertyChanged("IpAdress");
                }
            }
        }

        private string ipPort;
        public string IpPort
        {
            get
            {
                return ipPort;
            }
            set
            {
                if (ipPort != value)
                {
                    ipPort = value;
                    OnPropertyChanged("IpPort");
                }
            }
        }

        private int sampleTime;
        public string SampleTime
        {
            get
            {
                return sampleTime.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (sampleTime != st)
                    {
                        sampleTime = st;
                        OnPropertyChanged("SampleTime");
                    }
                }
            }
        }
        private int maxSamples;
        public string MaxSamples
        {
            get
            {
                return maxSamples.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int ms))
                {
                    if (maxSamples != ms)
                    {
                        maxSamples = ms;
                        OnPropertyChanged("MaxSamples");
                    }
                }
            }
        }
        private string apiVersion;
        public string ApiVersion
        {
            get
            {
                return apiVersion;
            }
            set
            {
                if (apiVersion != value)
                {
                    apiVersion = value;
                    OnPropertyChanged("ApiValue");
                }
            }
        }
        public ButtonCommand SaveButton { get; set; }
        public ButtonCommand DefaultButton { get; set; }
        #endregion
        #region Fields
        private ConfigParams config = new ConfigParams();

        #endregion

        public View2_ViewModel()
        {
            ipAddress = config.IpAddress;
            ipPort = config.IpPort;
            sampleTime = config.SampleTime;
            maxSamples = config.MaxSamples;
            apiVersion = config.ApiVersion;

            SaveButton = new ButtonCommand(SaveSettings);
            DefaultButton = new ButtonCommand(DefaultSettings);
        }

        public void SaveSettings()
        {
            Debug.WriteLine("Save Button Works!");
            config = new ConfigParams(ipAddress, ipPort, apiVersion, maxSamples, sampleTime);
            config.SaveConfigToFile();
        }
        public void DefaultSettings()
        {
            Console.WriteLine("Default Button Works!");
            config.SetDefaultConfig();
            config.SaveConfigToFile();

            IpAddress = config.IpAddress;
            IpPort = config.IpPort;
            SampleTime = (config.SampleTime).ToString();
            MaxSamples = (config.MaxSamples).ToString();
            ApiVersion = config.ApiVersion;


        }


    }
}