using System;
using System.Runtime.Serialization;

namespace DataLaag.Exceptions
{
    [Serializable]
    public class ContinentRepositoryADOException : Exception
    {
        public ContinentRepositoryADOException()
        {
        }

        public ContinentRepositoryADOException(string message) : base(message)
        {
        }

        public ContinentRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContinentRepositoryADOException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
