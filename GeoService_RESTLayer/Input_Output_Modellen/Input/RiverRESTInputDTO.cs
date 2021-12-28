using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Input_Output_Modellen.Input {
    public class RiverRESTInputDTO {
        public int RiverId { get; set; }
        public string Name { get; set; }

        public int Length { get; set; }

        public List<int> CountryIds { get; set; }
    }
}
