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
        public PlotModel DataPlotModel4 { get; set; }
        public PlotModel DataPlotModel5 { get; set; }
        public PlotModel DataPlotModel6 { get; set; }
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
                Minimum = 0,
                Maximum = 100,
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
                Minimum = -30,
                Maximum = 105,
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
                Minimum = 260,
                Maximum = 1260,
                Key = "Vertical",
                Unit = "mbar",
                Title = "Temperature"
            });
            DataPlotModel3.Series.Add(new LineSeries() { Title = "random pressure series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 3rd plot

            //4th plot (dodane przeze mnie)
            DataPlotModel4 = new PlotModel { Title = "YAW" };

            DataPlotModel4.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel4.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                Key = "Vertical",
                Unit = "degree",
                Title = "Yaw"
            });
            DataPlotModel4.Series.Add(new LineSeries() { Title = "random yaw series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 4th plot

            //5th plot (dodane przeze mnie)
            DataPlotModel5 = new PlotModel { Title = "PITCH" };

            DataPlotModel5.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel5.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                Key = "Vertical",
                Unit = "degree",
                Title = "Pitch"
            });
            DataPlotModel5.Series.Add(new LineSeries() { Title = "random pitch series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 5th plot

            //6th plot (dodane przeze mnie)
            DataPlotModel6 = new PlotModel { Title = "ROLL" };

            DataPlotModel6.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            DataPlotModel6.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                Key = "Vertical",
                Unit = "degree",
                Title = "Roll"
            });
            DataPlotModel6.Series.Add(new LineSeries() { Title = "random roll series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 6th plot



            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;

            Server = new IoTServer(IpAddress);
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

            if (lineSeries.Points.Count > config.MaxSampleNumber)
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

            if (lineSeries.Points.Count > config.MaxSampleNumber)
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

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel3.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel3.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel3.InvalidatePlot(true);
        }

        /**
         *FUNKCJA STWORZONA PRZEZE MNIE ODPOWIEDZIALNA ZA ODŚWIEŻANIE WYKRESU
         */
        private void UpdatePlot4(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel4.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel4.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel4.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel4.InvalidatePlot(true);
        }

        /**
          *FUNKCJA STWORZONA PRZEZE MNIE ODPOWIEDZIALNA ZA ODŚWIEŻANIE WYKRESU
          */
        private void UpdatePlot5(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel5.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel5.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel5.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel5.InvalidatePlot(true);
        }

        /**
          *FUNKCJA STWORZONA PRZEZE MNIE ODPOWIEDZIALNA ZA ODŚWIEŻANIE WYKRESU
          */
        private void UpdatePlot6(double t, double d)
        {
            LineSeries lineSeries = DataPlotModel6.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                DataPlotModel6.Axes[0].Minimum = (t - config.XAxisMax);
                DataPlotModel6.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            DataPlotModel6.InvalidatePlot(true);
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
            string responseText4 = await Server.GETwithClient4();
            string responseText5 = await Server.GETwithClient5();
            string responseText6 = await Server.GETwithClient6();
#else
            string responseText = await Server.POSTwithClient();
            string responseText2 = await Server.POSTwithClient2();
            string responseText3 = await Server.POSTwithClient3();
            string responseText4 = await Server.POSTwithClient4();
            string responseText5 = await Server.POSTwithClient5();
            string responseText6 = await Server.POSTwithClient6();
#endif
#else
#if GET
            string responseText = await Server.GETwithRequest();
            string responseText2 = await Server.GETwithRequest2();
            string responseText3 = await Server.GETwithRequest3();
            string responseText4 = await Server.GETwithRequest4();
            string responseText5 = await Server.GETwithRequest5();
            string responseText6 = await Server.GETwithRequest6();
#else
            string responseText = await Server.POSTwithRequest();
            string responseText2 = await Server.POSTwithRequest2();
            string responseText3 = await Server.POSTwithRequest3();
            string responseText4 = await Server.POSTwithRequest4();
            string responseText5 = await Server.POSTwithRequest5();
            string responseText6 = await Server.POSTwithRequest6();
#endif
#endif
            try
            {
#if DYNAMIC
                dynamic resposneJson = JObject.Parse(responseText);
                UpdatePlot(timeStamp / 1000.0, (double)resposneJson.data);
                dynamic resposneJson2 = JObject.Parse(responseText2);
                UpdatePlot2(timeStamp / 1000.0, (double)resposneJson2.data);
#else
                ServerData resposneJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlot(timeStamp / 1000.0, resposneJson.data);
                ServerData resposneJson2 = JsonConvert.DeserializeObject<ServerData>(responseText2);
                UpdatePlot2(timeStamp / 1000.0, resposneJson2.data);
                ServerData resposneJson3 = JsonConvert.DeserializeObject<ServerData>(responseText3);
                UpdatePlot3(timeStamp / 1000.0, resposneJson3.data);
                ServerData resposneJson4 = JsonConvert.DeserializeObject<ServerData>(responseText4);
                UpdatePlot4(timeStamp / 1000.0, resposneJson4.data);
                ServerData resposneJson5 = JsonConvert.DeserializeObject<ServerData>(responseText5);
                UpdatePlot5(timeStamp / 1000.0, resposneJson5.data);
                ServerData resposneJson6 = JsonConvert.DeserializeObject<ServerData>(responseText6);
                UpdatePlot6(timeStamp / 1000.0, resposneJson6.data);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText);
                Debug.WriteLine(responseText2);
                Debug.WriteLine(responseText3);
                Debug.WriteLine(responseText4);
                Debug.WriteLine(responseText5);
                Debug.WriteLine(responseText6);
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
            Server = new IoTServer(IpAddress);

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
            SampleTime = config.SampleTime.ToString();
            Server = new IoTServer(IpAddress);

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