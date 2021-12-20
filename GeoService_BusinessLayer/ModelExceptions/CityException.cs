using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.ModelExceptions
{
    public class CityException : Exception
    {
        public CityException()
        {
        }

        public CityException(string message) : base(message)
        {
        }

        public CityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
