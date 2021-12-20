using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Interfaces
{
    public interface ICountryRepository
    {
        List<Country> GeefLandenContinent(int id);
        bool HeeftLanden(int continentId);
        Country LandToevoegen(Country land);
        bool BestaatLand(int landId);
        Country LandWeergeven(int landId);
        void LandVerwijderen(int landId);
        Country LandUpdaten(Country land);
        bool BestaatLand(string naam, int id);
        bool ZitLandInContinent(int continentId, int landId);
    }
}
