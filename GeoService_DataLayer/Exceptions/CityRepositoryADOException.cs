using System;
using System.Runtime.Serialization;

namespace DataLaag.Exceptions
{
    [Serializable]
    public class CityRepositoryADOException : Exception
    {
        public CityRepositoryADOException()
        {
        }

        public CityRepositoryADOException(string message) : base(message)
        {
        }

        public CityRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CityRepositoryADOException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
