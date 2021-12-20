using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class CountryServiceException : Exception
    {
        public CountryServiceException()
        {
        }

        public CountryServiceException(string message) : base(message)
        {
        }

        public CountryServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountryServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
