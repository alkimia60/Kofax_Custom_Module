using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;



namespace CassCustomModuleService
{
    class UtilsRestService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Returns JSON string
        public string getRestString(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String res = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                    //log.Info(res);
                    return res;
                }
               
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    log.Debug(errorText);
                    reader.Close();
                    responseStream.Close();
                }
                throw;
            }
        }
        //Returns Object class
        public PostClass getRestObject(string url)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                    String res = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();

                    PostClass deserializedPostClass = JsonConvert.DeserializeObject<PostClass>(res);

                    //log.Info("Body: " + deserializedPostClass.body);
                    //log.Info("title: " + deserializedPostClass.title);
                    //log.Info("ID" + deserializedPostClass.id);
                    //log.Info("userID: " + deserializedPostClass.userID);


                    return deserializedPostClass;
                }

            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    log.Debug(errorText);
                    reader.Close();
                    responseStream.Close();
                }
                throw;
            }
           
        }
        public string postInfo() 
        {
            // Create a request using a URL that can receive a post. 
             WebRequest request = WebRequest.Create ("http://jsonplaceholder.typicode.com/posts");
             // Set the Method property of the request to POST.
             request.Method = "POST";
             // Create POST data and convert it to a byte array.
             string postData = "{ title: \'foo\', body: \'bar\',userId: 1}";
             byte[] byteArray = Encoding.UTF8.GetBytes(postData);
             // Set the ContentType property of the WebRequest.
             request.ContentType = "application/x-www-form-urlencoded";
             // Set the ContentLength property of the WebRequest.
             request.ContentLength = byteArray.Length;
             // Get the request stream.
             Stream dataStream = request.GetRequestStream();
             // Write the data to the request stream.
             dataStream.Write(byteArray, 0, byteArray.Length);
             // Close the Stream object.
             dataStream.Close();
             // Get the response.
             WebResponse response = request.GetResponse();
             // Get the stream containing content returned by the server.
             dataStream = response.GetResponseStream();
             log.Info("Respuesta WEB: " + ((HttpWebResponse)response).StatusDescription.ToString() +  ((HttpWebResponse)response).StatusCode.ToString());
             // Open the stream using a StreamReader for easy access.
             StreamReader reader = new StreamReader(dataStream);
             // Read the content.
             string responseFromServer = reader.ReadToEnd();
            // Display the content.
             //log.Info(responseFromServer);
             // Clean up the streams.
             reader.Close();
             dataStream.Close();
             response.Close();

             return ": 200";

        }
    }
}
