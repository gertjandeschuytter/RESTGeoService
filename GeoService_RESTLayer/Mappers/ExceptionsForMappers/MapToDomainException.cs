using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Mappers.ExceptionsForMappers {
    public class MapToDomainException : Exception {
        public MapToDomainException()
        {
        }

        public MapToDomainException(string message) : base(message)
        {
        }

        public MapToDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MapToDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
