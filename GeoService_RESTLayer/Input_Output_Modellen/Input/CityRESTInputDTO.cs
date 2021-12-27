using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Input {
    public class CityRESTInputDTO {
        public string Name { get; set; }
        public int Population { get; set; }
        public bool IsCapital { get; set; }
        public int ContinentId { get; set; }
        public int CountryId { get; set; }
    }
}
