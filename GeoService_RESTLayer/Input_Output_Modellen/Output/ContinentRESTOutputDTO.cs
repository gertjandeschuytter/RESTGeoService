using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Input {
    public class ContinentRESTOutputDTO {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public int amount_countries { get; set; }
        public List<string> Countries { get; set; }

        public ContinentRESTOutputDTO(string id, string naam, int bevolkingsaantal, int aantalLanden, List<string> landen)
        {
            Id = id;
            Name = naam;
            Population = bevolkingsaantal;
            amount_countries = aantalLanden;
            Countries = landen;
        }

        public ContinentRESTOutputDTO()
        {
        }
    }
}
