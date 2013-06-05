using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace CloudlogCAT.API
{
    internal class CloudlogAPI
    {
        private JavaScriptSerializer m_Serializer;

        private string m_UrlBase;

        public CloudlogAPI(string baseUrl)
        {
            m_Serializer = new JavaScriptSerializer();
            m_Serializer.RegisterConverters(new List<JavaScriptConverter> { new CATModel.Converter() });
            m_UrlBase = baseUrl.TrimEnd(' ', '/') + "/index.php/api/radio";
        }

        public void PushCAT(CATModel cat)
        {
            string catJson = m_Serializer.Serialize(cat);
            string response = PostRequest(m_UrlBase, catJson, "application/json");
        }

        private static string PostRequest(string url, string body, string bodyContentType)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            using (StreamWriter writer = new StreamWriter(req.GetRequestStream()))
            {
                writer.Write(body);
            }
            req.ContentType = bodyContentType;
            try
            {
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
