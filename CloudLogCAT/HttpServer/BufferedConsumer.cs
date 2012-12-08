using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kayak;
using System.IO;

namespace CloudlogCAT.HttpServer
{
    internal class BufferedConsumer : IDataConsumer
    {
        MemoryStream m_DataBuffer = new MemoryStream();
        Action<Stream> resultCallback;
        Action<Exception> errorCallback;

        public BufferedConsumer(Action<Stream> resultCallback, Action<Exception> errorCallback)
        {
            this.resultCallback = resultCallback;
            this.errorCallback = errorCallback;
        }
        public bool OnData(ArraySegment<byte> data, Action continuation)
        {
            // since we're just buffering, ignore the continuation. 
            // TODO: place an upper limit on the size of the buffer. 
            // don't want a client to take up all the RAM on our server! 
            m_DataBuffer.Write(data.Array, data.Offset, data.Count);
            return false;
        }
        public void OnError(Exception error)
        {
            errorCallback(error);
        }

        public void OnEnd()
        {
            resultCallback(m_DataBuffer);
        }
    }
}
