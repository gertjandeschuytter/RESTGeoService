using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Output {
    public class CityRESTOuputDTO {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public bool IsCapital { get; set; }
        public string ContinentId { get; set; }
        public string CountryId { get; set; }

        public CityRESTOuputDTO(string id, string name, int population, bool isCapital, string continentId, string countryId)
        {
            Id = id;
            Name = name;
            Population = population;
            IsCapital = isCapital;
            ContinentId = continentId;
            CountryId = countryId;
        }
    }
}
