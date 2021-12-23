using DataLaag.ADO;
using GeoService_BusinessLayer.Models;
using System;
using System.Configuration;

namespace ADOTestOmgeving
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["GeoServiceDB"].ConnectionString;
            ContinentRepositoryADO continentADO = new(connectionString);
            CountryRepositoryADO countryADO = new(connectionString);
            CityRepositoryADO cityADO = new(connectionString);

            //continentADO

            //bool bestaatContinent = continentADO.BestaatContinent(1);
            //Continent ToegevoegdContinent = continentADO.ContinentToevoegen(new Continent("RandomContinent", 1000000));
            //Continent cont1 = continentADO.ContinentWeergeven(1003);
            //cont1.ZetNaam("RandomContinentAangepast");
            //continentADO.ContinentUpdaten(cont1);
            //continentADO.ContinentVerwijderen(1002);

            //countryADO
            //var landen = countryADO.GeefLandenContinent(1);
            //bool heeftlanden = countryADO.HeeftLanden(1);
            //var country = countryADO.LandToevoegen(new("randomland2", 1000000, 50000, cont1));
            //countryADO.LandVerwijderen(1002);
            //var country = countryADO.LandWeergeven(1003);
            //country.ZetNaam("Randomland5");
            //bool bestaat1 = countryADO.BestaatLand(2);
            //Country countryUpdated = countryADO.LandUpdaten(country);
            //bool zitlandinCon = countryADO.ZitLandInContinent(1, 2);

            //cityADO
            //bool bestaatstad = cityADO.BestaatStad(1);
            //bool controleercontinentlandstadbevolking = cityADO.ControleerBevolkingsaantal(1, 1000000);
            //var steden = cityADO.GeefStedenLand(2);
            //bool heefsteden = cityADO.HeeftSteden(2);
            //var stadje = cityADO.StadWeergeven(1002);
            //stadje.ZetBevolkingsaantal(50000);
            //toevoegen werkt
            //cityADO.StadVerwijderen(1004);
            //cityADO.StadUpdaten(stadje);
            //cityADO.ZitStadInLandInContinent(1003, 1003, 1005);
        }
    }
}
