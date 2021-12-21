using System;
using System.Runtime.Serialization;

namespace DataLaag.Exceptions
{
    [Serializable]
    public class CountryRepositoryADOException : Exception
    {
        public CountryRepositoryADOException()
        {
        }

        public CountryRepositoryADOException(string message) : base(message)
        {
        }

        public CountryRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountryRepositoryADOException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
