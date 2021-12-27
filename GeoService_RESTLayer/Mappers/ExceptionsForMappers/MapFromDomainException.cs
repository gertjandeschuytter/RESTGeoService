using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Mappers.ExceptionsForMappers {
    public class MapFromDomainException : Exception {
        public MapFromDomainException()
        {
        }

        public MapFromDomainException(string message) : base(message)
        {
        }

        public MapFromDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MapFromDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
