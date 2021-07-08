using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiViewApp.Model
{

    public class IoTServer
    {
        private string ip;
        private string port;

        public IoTServer(string _ip, string _port)
        {
            ip = _ip;
            port = _port;
        }

        /**
        * @brief obtaining the address of the PHP script from IoT server IP.
        * @return LED display control script URL
        */
        public string ScriptUrl
        {
            get => "http://" + ip + ":" + port + "/led_display.php";
        }

        public async Task<string> PostControlRequest(List<KeyValuePair<string, string>> data)
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var requestData = new FormUrlEncodedContent(data);
                    // Sent POST request
                    var result = await client.PostAsync(ScriptUrl, requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }

        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetFileUrl_Roll()
        {
            return "http://" + ip + ":" + port +"/chartdata_desktop6.json";
        }
        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetFileUrl_Pitch()
        {
            return "http://" + ip + ":" + port + "/chartdata_desktop5.json";
        }
        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetFileUrl_Yaw()
        {
            return "http://" + ip + ":" + port + "/chartdata_desktop4.json";
        }
        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetFileUrl_Humidity()
        {
            return "http://" + ip + ":" + port + "/chartdata_desktop.json";
        }
        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetFileUrl_Temperature()
        {
            return "http://" + ip + ":" + port + "/chartdata_desktop2.json";
        }

        /**
          * @brief obtaining the address of the data file from IoT server IP.
          */
        private string GetFileUrl_Pressure()
        {
            return "http://" + ip + ":" + port + "/chartdata_desktop3.json";
        }

        /**
         * @brief obtaining the address of the PHP script from IoT server IP.
         */
        private string GetScriptUrl()
        {
            return "http://" + ip + ":" + port + "/projektAM/resource_desktop.php";
        }


        /**
         * @brief HTTP GET request using HttpClient
         */
        public async Task<string> GETwithClient_Roll()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Roll());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
         * @brief HTTP GET request using HttpClient
         */
        public async Task<string> GETwithClient_Pitch()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Pitch());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpClient
        */
        public async Task<string> GETwithClient_Yaw()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Yaw());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
          * @brief HTTP GET request using HttpClient
          */
        public async Task<string> GETwithClient_Humidity()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Humidity());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpClient
        */
        public async Task<string> GETwithClient_Temperature()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Temperature());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }

        /**
        * @brief HTTP GET request using HttpClient
        */
        public async Task<string> GETwithClient_Pressure()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetFileUrl_Pressure());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }


        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Roll()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop6"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Pitch()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop5"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }

        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Yaw()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop4"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Humidity()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Temperature()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop2"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpClient
        */
        public async Task<string> POSTwithClient_Pressure()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // POST request data
                    var requestDataCollection = new List<KeyValuePair<string, string>>();
                    requestDataCollection.Add(new KeyValuePair<string, string>("filename", "chartdata_desktop3"));
                    var requestData = new FormUrlEncodedContent(requestDataCollection);
                    // Sent POST request
                    var result = await client.PostAsync(GetScriptUrl(), requestData);
                    // Read response content
                    responseText = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
          * @brief HTTP GET request using HttpWebRequest
          */
        public async Task<string> GETwithRequest_Roll()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Roll());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETwithRequest_Pitch()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Pitch());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETwithRequest_Yaw()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Yaw());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETwithRequest_Humidity()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Humidity());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETwithRequest_Temperature()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Temperature());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETwithRequest_Pressure()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetFileUrl_Pressure());

                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }


       
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Humidity()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Yaw()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop4";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Pitch()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop5";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Roll()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop6";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Temperature()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop2";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }
        /**
        * @brief HTTP POST request using HttpWebRequest
        */
        public async Task<string> POSTwithRequest_Pressure()
        {
            string responseText = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetScriptUrl());

                // POST Request data 
                var requestData = "filename=chartdata_desktop3";
                byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
                // POST Request configuration
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                // Wrire data to request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);
            }

            return responseText;
        }


        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetDataUri()
        {
            return "http://" + ip + ":" + port + "/datalist.json";
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETData()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetDataUri());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);


            }

            return responseText;
        }

        /**
         * @brief obtaining the address of the data file from IoT server IP.
         */
        private string GetJoystickUri()
        {
            return "http://" + ip + ":" + port + "/datajoystick123.json";
        }
        /**
        * @brief HTTP GET request using HttpWebRequest
        */
        public async Task<string> GETJoystick()
        {
            string responseText = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    responseText = await client.GetStringAsync(GetJoystickUri());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("NETWORK ERROR");
                Debug.WriteLine(e);


            }

            return responseText;
        }
    }
}
