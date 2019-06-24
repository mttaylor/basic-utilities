using System;
using System.IO;
using System.Net;
using System.Text;
   
namespace DownloadSlaveAgent
{
    class DownloadJNLP
    {
        static void Main()
        {
            WebClient WC = new WebClient();
            //WC.Credentials = new NetworkCredential("mtaylor", "BlahBlah21!", "Opto22.com");
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("mtaylor" + ":" + "BlahBlah21!"));
            client.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);
            WC.DownloadFile("https://jenkins-w7/computer/RanorexTestVM/slave-agent.jnlp", "C:\\Desktop");

            


            //Uri url = new Uri("https://jenkins-w7/computer/RanorexTestVM/slave-agent.jnlp");
            //HttpWebRequest request = null;

            //ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            //CookieContainer cookieJar = new CookieContainer();
      
            //request = (HttpWebRequest)WebRequest.Create(url);
            //request.CookieContainer = cookieJar;
            //request.Method = "GET";
            //HttpStatusCode responseStatus;

            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    responseStatus = response.StatusCode;
            //    url = request.Address;
            //}

            //if (responseStatus == HttpStatusCode.OK)
            //{
            //    UriBuilder urlBuilder = new UriBuilder(url);
            //    urlBuilder.Path = urlBuilder.Path.Remove(urlBuilder.Path.LastIndexOf('/')) + "/j_security_check";

            //    request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString());
            //    request.Referer = url.ToString();
            //    request.CookieContainer = cookieJar;
            //    request.Method = "POST";
            //    request.ContentType = "application/x-www-form-urlencoded";

            //    using (Stream requestStream = request.GetRequestStream())
            //    using (StreamWriter requestWriter = new StreamWriter(requestStream, Encoding.ASCII))
            //    {
            //        string postData = "j_username=mtaylor&j_password=BlahBlah21!&submit=Send";
            //        requestWriter.Write(postData);
            //    }

            //    string responseContent = null;

            //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //    using (Stream responseStream = response.GetResponseStream())
            //    using (StreamReader responseReader = new StreamReader(responseStream))
            //    {
            //        responseContent = responseReader.ReadToEnd();
            //    }

            //    Console.WriteLine(responseContent);
            //}
            //else
            //{
            //    Console.WriteLine("Client was unable to connect!");
            //}     

        }     
    }


    

   
   
}

