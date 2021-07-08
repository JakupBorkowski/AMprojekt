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
      * @brief View model for View1_ViewModel.xaml 
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

        public PlotModel humidityGraph { get; set; } //!< Data plot model 
        public PlotModel temperatureGraph { get; set; } //!< Data plot model 
        public PlotModel pressureGraph { get; set; } //!< Data plot model 
        public PlotModel yawGraph { get; set; } //!< Data plot model 
        public PlotModel pitchGraph { get; set; } //!< Data plot model 
        public PlotModel rollGraph { get; set; } //!< Data plot model 

        public ButtonCommand StartButton { get; set; } //!< 'START' button command
        public ButtonCommand StopButton { get; set; } //!< 'STOP' button command
        public ButtonCommand UpdateConfigButton { get; set; } //!< 'UPDATE' button command
        public ButtonCommand DefaultConfigButton { get; set; } //!< 'DEFAULT' button command
        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion
        /**
        * @brief View1_ViewModel constructor.
        */
        public View1_ViewModel()
        {
            //poczatek9
            rollGraph = new PlotModel { Title = "ROLL" };

            rollGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            rollGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "deg",
                Title = "Roll"
            });
            rollGraph.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //koniec9

            //poczatek8
            pitchGraph = new PlotModel { Title = "PITCH" };

            pitchGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            pitchGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "deg",
                Title = "Pitch"
            });
            pitchGraph.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //koniec8


            //poczatek 7
            yawGraph = new PlotModel { Title = "YAW" };

            yawGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            yawGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 365,
                Key = "Vertical",
                Unit = "deg",
                Title = "Yaw"
            });
            yawGraph.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 7th plot

            humidityGraph = new PlotModel { Title = "HUMIDITY" };

            humidityGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            humidityGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -2,
                Maximum = 105,
                Key = "Vertical",
                Unit = "%",
                Title = "Humidity"
            });
            humidityGraph.Series.Add(new LineSeries() { Title = "random humidity series", Color = OxyColor.Parse("#FFFF0000") });

            //2nd plot (dodane przeze mnie)
            temperatureGraph = new PlotModel { Title = "TEMPERATURE" };

            temperatureGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            temperatureGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -32,
                Maximum = 107,
                Key = "Vertical",
                Unit = "*C",
                Title = "Temperature"
            });
            temperatureGraph.Series.Add(new LineSeries() { Title = "random temperature series", Color = OxyColor.Parse("#FFFF0000") });
            //end of 2nd plot

            //3rd plot (dodane przeze mnie)
            pressureGraph = new PlotModel { Title = "PRESSURE" };

            pressureGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            pressureGraph.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 250,
                Maximum = 1270,
                Key = "Vertical",
                Unit = "mbar",
                Title = "Pressure"
            });
            pressureGraph.Series.Add(new LineSeries() { Title = "random pressure series", Color = OxyColor.Parse("#FFFF0000") });
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
        private void UpdateRollPlot(double t, double d)
        {
            LineSeries lineSeries = rollGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                rollGraph.Axes[0].Minimum = (t - config.XAxisMax);
                rollGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            rollGraph.InvalidatePlot(true);
        }

        /**
         * @brief Time series plot update procedure.
         * @param t X axis data: Time stamp [ms].
         * @param d Y axis data: Real-time measurement [-].
         */
        private void UpdatePitchPlot(double t, double d)
        {
            LineSeries lineSeries = pitchGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                pitchGraph.Axes[0].Minimum = (t - config.XAxisMax);
                pitchGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            pitchGraph.InvalidatePlot(true);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdateYawPlot(double t, double d)
        {
            LineSeries lineSeries = yawGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                yawGraph.Axes[0].Minimum = (t - config.XAxisMax);
                yawGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            yawGraph.InvalidatePlot(true);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdateHumidityPlot(double t, double d)
        {
            LineSeries lineSeries = humidityGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                humidityGraph.Axes[0].Minimum = (t - config.XAxisMax);
                humidityGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            humidityGraph.InvalidatePlot(true);
        }

        /**
           * @brief Time series plot update procedure.
           * @param t X axis data: Time stamp [ms].
           * @param d Y axis data: Real-time measurement [-].
           */
        private void UpdateTemperaturePlot(double t, double d)
        {
            LineSeries lineSeries = temperatureGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                temperatureGraph.Axes[0].Minimum = (t - config.XAxisMax);
                temperatureGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            temperatureGraph.InvalidatePlot(true);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePressurePlot(double t, double d)
        {
            LineSeries lineSeries = pressureGraph.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSamples)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                pressureGraph.Axes[0].Minimum = (t - config.XAxisMax);
                pressureGraph.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            pressureGraph.InvalidatePlot(true);
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
       
            string HumidityResponseText = await Server.GETwithClient_Humidity();
            string TemperatureResponseText = await Server.GETwithClient_Temperature();
            string PressureResponseText = await Server.GETwithClient_Pressure();
            string YawResponseText = await Server.GETwithClient_Yaw();
            string PitchResponseText = await Server.GETwithClient_Pitch();
            string RollResponseText = await Server.GETwithClient_Roll();
#else
            string HumidityResponseText = await Server.GETwithClient_Humidity();
            string TemperatureResponseText = await Server.GETwithClient_Temperature();
            string PressureResponseText = await Server.GETwithClient_Pressure();
            string YawResponseText = await Server.GETwithClient_Yaw();
            string PitchResponseText = await Server.GETwithClient_Pitch();
            string RollResponseText = await Server.GETwithClient_Roll();
            
#endif
#else
#if GET
            string HumidityResponseText = await Server.GETwithRequest_Humidity();
            string TemperatureResponseText = await Server.GETwithRequest_Temperature();
            string PressureResponseText = await Server.GETwithRequest_Pressure();
            string YawResponseText = await Server.GETwithRequest_Yaw();
            string PitchResponseText = await Server.GETwithRequest_Pitch();
            string RollResponseText = await Server.GETwithRequest_Roll();
            
#else
            string HumidityResponseText = await Server.POSTwithRequest_Humidity();
            string TemperatureResponseText = await Server.POSTwithRequest_Temperature();
            string PressureResponseText = await Server.POSTwithRequest_Pressure();
            string YawResponseText = await Server.POSTwithRequest_Yaw();
            string PitchResponseText = await Server.POSTwithRequest_Pitch();
            string RollResponseText = await Server.POSTwithRequest_Roll();
            
#endif
#endif
            try
            {
#if DYNAMIC
                dynamic HumidityResposneJson = JObject.Parse(HumidityResponseText);
                UpdateHumidityPlot(timeStamp / 1000.0, (double)HumidityResposneJson.data);
                
                dynamic TemperatureResposneJson = JObject.Parse(TemperatureResponseText);
                UpdateHumidityPlot(timeStamp / 1000.0, (double)TemperatureResposneJson.data);

                dynamic PressureResposneJson = JObject.Parse(PressureResponseText);
                UpdatePressurePlot(timeStamp / 1000.0, (double)PressureResposneJson.data);

                dynamic YawResposneJson = JObject.Parse(YawResponseText);
                UpdateYawPlott(timeStamp / 1000.0, (double)YawResposneJson.data);

                dynamic PitchResposneJson = JObject.Parse(PitchResponseText);
                UpdatePitchPlot(timeStamp / 1000.0, (double)PitchResposneJson.data);

                dynamic RollResposneJson = JObject.Parse(RollResponseText);
                UpdateRollPlot(timeStamp / 1000.0, (double)RollResposneJson.data);
                              
#else

                ServerData HumidityResposneJson = JsonConvert.DeserializeObject<ServerData>(HumidityResponseText);
                UpdateHumidityPlot(timeStamp / 1000.0, HumidityResposneJson.data);
                ServerData TemperatureResposneJson = JsonConvert.DeserializeObject<ServerData>(TemperatureResponseText);
                UpdateTemperaturePlot(timeStamp / 1000.0, TemperatureResposneJson.data);
                ServerData PressureResposneJson = JsonConvert.DeserializeObject<ServerData>(PressureResponseText);
                UpdatePressurePlot(timeStamp / 1000.0, PressureResposneJson.data);
                ServerData YawResposneJson = JsonConvert.DeserializeObject<ServerData>(YawResponseText);
                UpdateYawPlot(timeStamp / 1000.0, YawResposneJson.data);
                ServerData PitchResposneJson = JsonConvert.DeserializeObject<ServerData>(PitchResponseText);
                UpdatePitchPlot(timeStamp / 1000.0, PitchResposneJson.data);
                ServerData RollResposneJson = JsonConvert.DeserializeObject<ServerData>(RollResponseText);
                UpdateRollPlot(timeStamp / 1000.0, RollResposneJson.data);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(HumidityResponseText);
                Debug.WriteLine(TemperatureResponseText);
                Debug.WriteLine(PressureResponseText);
                Debug.WriteLine(YawResponseText);
                Debug.WriteLine(PitchResponseText);
                Debug.WriteLine(RollResponseText);
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

                humidityGraph.ResetAllAxes();
                temperatureGraph.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                pressureGraph.ResetAllAxes();
               //analogiczne dodatnie resetu dla drugiego plota
                yawGraph.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                pitchGraph.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
                rollGraph.ResetAllAxes(); //analogiczne dodatnie resetu dla drugiego plota
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