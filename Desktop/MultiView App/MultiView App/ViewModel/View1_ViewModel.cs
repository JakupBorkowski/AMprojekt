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

namespace MultiViewApp.ViewModel
{
    using Model;

    /** 
      * @brief View model for MainWindow.xaml 
      */
    public class View1_ViewModel : BaseViewModel
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
                    OnPropertyChanged("IpAddress");
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

        public PlotModel DataPlotModel { get; set; }
        public PlotModel DataPlotModel2 { get; set; } //zmienna, którą dodałem potrzebna, żeby narysować wykres
        public PlotModel DataPlotModel3 { get; set; }
        public PlotModel DataPlotModel7 { get; set; }
        public PlotModel DataPlotModel8 { get; set; }
        public PlotModel DataPlotModel9 { get; set; }

        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }
        public ButtonCommand UpdateConfigButton { get; set; }
        public ButtonCommand DefaultConfigButton { get; set; }
        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion

        public View1_ViewModel()
        {
            //poczatek9
            DataPlotModel9 = new PlotModel { Title = "ROLL" };

            DataPlotModel9.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel9.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "%",
                Title = "Roll"
            });
            DataPlotModel9.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //koniec9

            //poczatek8
            DataPlotModel8 = new PlotModel { Title = "PITCH" };

            DataPlotModel8.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel8.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "%",
                Title = "Pitch"
            });
            DataPlotModel8.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //koniec8


            //poczatek 7
            DataPlotModel7 = new PlotModel { Title = "YAW" };

            DataPlotModel7.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel7.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "%",
                Title = "Yaw"
            });
            DataPlotModel7.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 7th plot

            DataPlotModel = new PlotModel { Title = "HUMIDITY" };

            DataPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 105,
                Key = "Vertical",
                Unit = "%",
                Title = "Humidity"
            });
            DataPlotModel.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });

            //2nd plot (dodane przeze mnie)
            DataPlotModel2 = new PlotModel { Title = "TEMPERATURE" };

            DataPlotModel2.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel2.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -32,
                Maximum = 107,
                Key = "Vertical",
                Unit = "*C",
                Title = "Temperature"
            });
            DataPlotModel2.Series.Add(new LineSeries() { Title = "random temperature series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 2nd plot

            //3rd plot (dodane przeze mnie)
            DataPlotModel3 = new PlotModel { Title = "PRESSURE" };

            DataPlotModel3.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel3.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 250,
                Maximum = 1270,
                Key = "Vertical",
                Unit = "mbar",
                Title = "Pressure"
            });
            DataPlotModel3.Series.Add(new LineSeries() { Title = "random pressure series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 3rd plot

           


            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);

            ipAddress = config.IpAddress;
            ipPort = config.IpPort;
            sampleTime = config.SampleTime;

            Server = new IoTServer(IpAddress,IpPort);
        }
        /**
         * @brief Time series plot update procedure.
         * @param t X axis data: Time stamp [ms].
         * @param d Y axis data: Real-time measurement [-].
         */
        private void UpdatePlot9(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel9.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel9.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel9.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel9.InvalidatePlot(true);
        }
        /**
         * @brief Time series plot update procedure.
         * @param t X axis data: Time stamp [ms].
         * @param d Y axis data: Real-time measurement [-].
         */
        private void UpdatePlot8(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel8.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel8.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel8.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel8.InvalidatePlot(true);
        }

        private void UpdatePlot7(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel7.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel7.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel7.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel7.InvalidatePlot(true);
        }
        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePlot(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel.InvalidatePlot(true);
        }

        /**
          *FUNKCJA STWORZONA PRZEZE MNIE ODPOWIEDZIALNA ZA ODŚWIEŻANIE WYKRESU
          */
        private void UpdatePlot2(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel2.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel2.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel2.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel2.InvalidatePlot(true);
        }

        /**
          *FUNKCJA STWORZONA PRZEZE MNIE ODPOWIEDZIALNA ZA ODŚWIEŻANIE WYKRESU
          */
        private void UpdatePlot3(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel3.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel3.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel3.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel3.InvalidatePlot(true);
        }

       

        /**
          * @brief Asynchronous chart update procedure with
          *        data obtained from IoT server responses.
          * @param ip IoT server IP address.
          */
        private async void UpdatePlotWithServerResponse()
        {
#if CLIENT
#if GET
       
            string responseText = await Server.GETwithClient();
            string responseText2 = await Server.GETwithClient2();
            string responseText3 = await Server.GETwithClient3();
            string responseText7 = await Server.GETwithClient7();
            string responseText8 = await Server.GETwithClient8();
            string responseText9 = await Server.GETwithClient9();
#else
            string responseText = await Server.POSTwithClient();
            string responseText2 = await Server.POSTwithClient2();
            string responseText3 = await Server.POSTwithClient3();
            string responseText7 = await Server.GETwithClient7();
            string responseText8 = await Server.GETwithClient8();
            
#endif
#else
#if GET
            string responseText = await Server.GETwithRequest();
            string responseText2 = await Server.GETwithRequest2();
            string responseText3 = await Server.GETwithRequest3();
            string responseText7 = await Server.GETwithClient7();
            string responseText8 = await Server.GETwithClient8();
            
#else
            string responseText = await Server.POSTwithRequest();
            string responseText2 = await Server.POSTwithRequest2();
            string responseText3 = await Server.POSTwithRequest3();
            string responseText7 = await Server.GETwithClient7();
            string responseText8 = await Server.GETwithClient8();
            
#endif
#endif
            try
            {
#if DYNAMIC
                dynamic resposneJson = JObject.Parse(responseText);
                UpdatePlot(timeStamp / 1000.0, (double)resposneJson.data);
                dynamic resposneJson2 = JObject.Parse(responseText2);
                UpdatePlot2(timeStamp / 1000.0, (double)resposneJson2.data);
                dynamic resposneJson3 = JObject.Parse(responseText3);
                UpdatePlot3(timeStamp / 1000.0, (double)resposneJson3.data);
                dynamic resposneJson7 = JObject.Parse(responseText7);
                UpdatePlot7(timeStamp / 1000.0, (double)resposneJson7.data);
                              
#else
                
                ServerData resposneJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlot(timeStamp / 1000.0, resposneJson.data);
                ServerData resposneJson2 = JsonConvert.DeserializeObject<ServerData>(responseText2);
                UpdatePlot2(timeStamp / 1000.0, resposneJson2.data);
                ServerData resposneJson3 = JsonConvert.DeserializeObject<ServerData>(responseText3);
                UpdatePlot3(timeStamp / 1000.0, resposneJson3.data);
                ServerData resposneJson7 = JsonConvert.DeserializeObject<ServerData>(responseText7);
                UpdatePlot7(timeStamp / 1000.0, resposneJson7.data);
                ServerData resposneJson8 = JsonConvert.DeserializeObject<ServerData>(responseText8);
                UpdatePlot8(timeStamp / 1000.0, resposneJson8.data);
                ServerData resposneJson9 = JsonConvert.DeserializeObject<ServerData>(responseText9);
                UpdatePlot9(timeStamp / 1000.0, resposneJson9.data);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);
                Debug.WriteLine(responseText2);
                Debug.WriteLine(responseText3);
                Debug.WriteLine(responseText7);
                Debug.WriteLine(responseText8);
                Debug.WriteLine(responseText9);
                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }

        /**
          * @brief Synchronous procedure for request queries to the IoT server.
          * @param sender Source of the event: RequestTimer.
          * @param e An System.Timers.ElapsedEventArgs object that contains the event data.
          */
        private void RequestTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponse();
        }

        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimer()
        {
            if (RequestTimer == null)
            {
                RequestTimer = new Timer(config.SampleTime);
                RequestTimer.Elapsed += new ElapsedEventHandler(RequestTimerElapsed);
                RequestTimer.Enabled = true;

                DataPlotModel.ResetAllAxes();
                DataPlotModel2.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                DataPlotModel3.ResetAllAxes();
               //analogiczne dodatnie resetu dla drugiego plota
                DataPlotModel7.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                DataPlotModel8.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                DataPlotModel9.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimer()
        {
            if (RequestTimer != null)
            {
                RequestTimer.Enabled = false;
                RequestTimer = null;
            }
        }

        /**
         * @brief Configuration parameters update
         */
        private void UpdateConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams(ipAddress, sampleTime);
            Server = new IoTServer(IpAddress,IpPort);

            if (restartTimer)
                StartTimer();
        }

        /**
          * @brief Configuration parameters defualt values
          */
        private void DefaultConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams();
            IpAddress = config.IpAddress;
            IpPort = config.IpPort;
            SampleTime = config.SampleTime.ToString();
            Server = new IoTServer(IpAddress, IpPort);

            if (restartTimer)
                StartTimer();
        }

        #endregion

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