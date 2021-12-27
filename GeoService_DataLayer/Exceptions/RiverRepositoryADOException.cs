using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_DataLayer.Exceptions {
    public class RiverRepositoryADOException : Exception {
        public RiverRepositoryADOException()
        {
        }

        public RiverRepositoryADOException(string message) : base(message)
        {
        }

        public RiverRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RiverRepositoryADOException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
