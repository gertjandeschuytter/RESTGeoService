using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Interfaces
{
    public interface IContinentRepository
    {
        Continent ContinentToevoegen(Continent continent);
        bool BestaatContinent(int continentId);
        bool BestaatContinent(string name);
        Continent ContinentWeergeven(int continentId);
        void ContinentVerwijderen(int continentId);
        void ContinentUpdaten(Continent continent);
    }
}
