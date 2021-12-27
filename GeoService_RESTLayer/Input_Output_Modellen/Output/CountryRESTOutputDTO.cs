using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Output {
    public class CountryRESTOutputDTO {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public decimal Surface { get; set; }
        public string ContinentId { get; set; }
        public List<string> Capitals { get; set; }
        public List<string> Cities { get; set; }

        public CountryRESTOutputDTO(string id, string naam, int bevolkingsaantal, decimal oppervlakte, string continentId, List<string> capitals , List<string> cities)
        {
            Id = id;
            Name = naam;
            Population = bevolkingsaantal;
            Surface = oppervlakte;
            ContinentId = continentId;
            Capitals = capitals;
            Cities = cities;
        }
    }
}
