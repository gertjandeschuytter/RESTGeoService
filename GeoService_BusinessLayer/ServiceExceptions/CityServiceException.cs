using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class CityServiceException : Exception
    {
        public CityServiceException()
        {
        }

        public CityServiceException(string message) : base(message)
        {
        }

        public CityServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CityServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
