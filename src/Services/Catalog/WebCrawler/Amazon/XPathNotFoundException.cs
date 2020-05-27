using System;
using System.Runtime.Serialization;

namespace WebCrawler.Amazon
{
    [Serializable]
    internal class XPathNotFoundException : Exception
    {
        public XPathNotFoundException()
        {
        }

        public XPathNotFoundException(string message) : base(message)
        {
        }

        public XPathNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XPathNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}