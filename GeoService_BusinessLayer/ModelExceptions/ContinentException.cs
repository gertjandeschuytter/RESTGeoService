using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.ModelExceptions
{
    public class ContinentException : Exception
    {
        public ContinentException()
        {
        }

        public ContinentException(string message) : base(message)
        {
        }

        public ContinentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContinentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
