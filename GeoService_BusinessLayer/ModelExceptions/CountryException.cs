using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.ModelExceptions
{
    public class CountryException : Exception
    {
        public CountryException()
        {
        }

        public CountryException(string message) : base(message)
        {
        }

        public CountryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
