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
namespace MultiViewApp.ViewModel
{
    public class View2_ViewModel : BaseViewModel
    {
        #region Properties      
        private string _ipaddressText;
        public string ipaddressText
        {
            get
            {
                return (_ipaddressText);
            }
            set
            {
                if (value != _ipaddressText)
                {
                    _ipaddressText = value;
                    OnPropertyChanged("ipaddressText");
                }
            }
        }
        private string _portText;
        public string portText
        {
            get
            {
                return (_portText);
            }
            set
            {
                if (value != _portText)
                {
                    _portText = value;
                    OnPropertyChanged("portText");
                }
            }
        }
        private string _apiversionText;
        public string apiversionText
        {
            get
            {
                return (_apiversionText);
            }
            set
            {
                if (value != _apiversionText)
                {
                    _apiversionText = value;
                    OnPropertyChanged("apiversionText");
                }
            }
        }
        private string _tsText;
        public string tsText
        {
            get
            {
                return (_tsText);
            }
            set
            {
                if (value != _tsText)
                {
                    _tsText = value;
                    OnPropertyChanged("tsText");
                }
            }
        }
        private string _maxsampleText;
        public string maxsampleText
        {
            get
            {
                return (_maxsampleText);
            }
            set
            {
                if (value != _maxsampleText)
                {
                    _maxsampleText = value;
                    OnPropertyChanged("maxsampleText");
                }
            }
        }
        public ButtonCommand ChangeParametersButton { get; set; }  //!< 'Submit' button command
        #endregion

        #region Fields
        #endregion


        public View2_ViewModel()
        {
            ReadParameters();
            ChangeParametersButton = new ButtonCommand(UpdateParameters);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdateParameters()
        {      
            string parametersToJson = "[" + ipaddressText.ToString() + "," + portText.ToString() + "," + apiversionText.ToString() + "," + tsText.ToString() + "," + maxsampleText.ToString() + "]";
            System.IO.File.WriteAllText("jsonData.json", parametersToJson);
        }
        private void ReadParameters()
        {
            if (File.Exists("jsonData.json"))
            {
                string jsonStringToRead = System.IO.File.ReadAllText("jsonData.json");
                //string [] responseJson = JsonConvert.DeserializeObject<string[]>(jsonStringToRead);
                string[] responseJson = JSONDecoders.DecodeJsStringArray(jsonStringToRead);
                _ipaddressText = responseJson[0].ToString();
                _portText = responseJson[1].ToString();
                _apiversionText = responseJson[2].ToString();
                _tsText = responseJson[3].ToString();
                _maxsampleText = responseJson[4].ToString();
            }

        }


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}