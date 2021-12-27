using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Interfaces {
    public interface IRiverRepository {
        List<Country> GeefLandenRivier(int id);
        River RivierWeergeven(int id);
        bool BestaatRivier(int riverId);
        bool BestaatRivier(string name);
        River RivierToevoegen(River river);
        void RivierVerwijderen(int riverId);
    }
}
