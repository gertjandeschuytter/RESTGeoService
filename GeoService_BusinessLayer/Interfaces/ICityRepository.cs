using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Interfaces
{
    public interface ICityRepository
    {
        List<City> GeefStedenLand(int id);
        bool HeeftSteden(int countryId);
        City StadToevoegen(City city);
        bool BestaatStad(int id);
        City StadWeergeven(int cityId);
        void StadVerwijderen(int cityId);
        City StadUpdaten(City city);
        bool ControleerBevolkingsaantal(int countryId, int population);
        bool ZitStadInLandInContinent(int continentId, int countryId, int cityId);
    }
}
