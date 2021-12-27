using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.ServiceExceptions {
    public class RiverServiceException : Exception {
        public RiverServiceException()
        {
        }

        public RiverServiceException(string message) : base(message)
        {
        }

        public RiverServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RiverServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
