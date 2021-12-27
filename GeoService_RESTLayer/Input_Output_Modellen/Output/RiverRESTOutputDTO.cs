using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Output {
    public class RiverRESTOutputDTO {
        public RiverRESTOutputDTO(string riverId, string name, int length, List<string> countries)
        {
            RiverId = riverId;
            Name = name;
            Length = length;
            Countries = countries;
        }

        public string RiverId { get; set; }

        public string Name { get; set; }

        public int Length { get; set; }

        public List<string> Countries { get; set; } = new List<string>();
    }
}
