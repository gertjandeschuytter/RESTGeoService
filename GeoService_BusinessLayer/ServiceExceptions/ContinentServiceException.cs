using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class ContinentServiceException : Exception
    {
        public ContinentServiceException()
        {
        }

        public ContinentServiceException(string message) : base(message)
        {
        }

        public ContinentServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContinentServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
