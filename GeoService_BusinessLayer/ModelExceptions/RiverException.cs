using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.ModelExceptions {
    public class RiverException : Exception {
        public RiverException()
        {
        }

        public RiverException(string message) : base(message)
        {
        }

        public RiverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RiverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
