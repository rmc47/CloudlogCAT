using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Kayak.Http;
using Nancy.Helpers;
using System.IO;

namespace CloudlogCAT.HttpServer
{
    internal sealed class NancyRequestDelegate : IHttpRequestDelegate
    {
        private INancyEngine m_Engine;

        public NancyRequestDelegate(INancyEngine engine)
        {
            m_Engine = engine;
        }

        public void OnRequest(HttpRequestHead head, Kayak.IDataProducer body, IHttpResponseDelegate response)
        {
            BufferedConsumer requestConsumer = new BufferedConsumer(bodyStream => RequestReadCompleted(head, bodyStream, response), RequestReadError);
            body.Connect(requestConsumer);
        }

        private void RequestReadCompleted(HttpRequestHead head, Stream requestStream, IHttpResponseDelegate response)
        {
#warning wrong hard-coded base URI
            Uri baseUri = new Uri("http://localhost:8083/");
            Uri requestUrl = new Uri(baseUri, head.Uri);
            Url nancyUrl = new Url
            {
                Scheme = requestUrl.Scheme,
                HostName = requestUrl.Host,
                Port = requestUrl.IsDefaultPort ? null : (int?)requestUrl.Port,
                BasePath = baseUri.AbsolutePath.TrimEnd('/'),
                Path = HttpUtility.UrlDecode(head.Uri),
                Query = requestUrl.Query,
                Fragment = requestUrl.Fragment,
            };
            Dictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();
            foreach (var kvp in head.Headers)
                headers[kvp.Key] = new List<string> { kvp.Value };

            Request req = new Request(
                head.Method,
                nancyUrl,
                Nancy.IO.RequestStream.FromStream(requestStream, requestStream.Length),
                headers,
                null);

            m_Engine.HandleRequest(req, context => RequestProcessingCompleted(context, response), ex => RequestProcessingError(ex, response));
        }

        private void RequestReadError(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void RequestProcessingCompleted(NancyContext context, IHttpResponseDelegate response)
        {
            HttpResponseHead responseHead = new HttpResponseHead {
                Headers = context.Response.Headers,
                Status = context.Response.StatusCode.ToString()
            };

            byte[] responseBodyData;
            using (MemoryStream ms = new MemoryStream())
            {
                context.Response.Contents(ms);
                //ms.Seek(0, SeekOrigin.Begin);
                responseBodyData = ms.ToArray();
            }

            responseHead.Headers["Content-Type"] = context.Response.ContentType;
            responseHead.Headers["Content-Length"] = responseBodyData.LongLength.ToString();

            BufferedProducer bodyDataProducer = new BufferedProducer (responseBodyData);
            response.OnResponse(responseHead, bodyDataProducer);
        }

        private void RequestProcessingError(Exception ex, IHttpResponseDelegate response)
        {
            throw new NotImplementedException();
        }
    }
}
