using DomeinLaag.Services;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.Services;
using GeoService_RESTLayer.Input_Output_Modellen.Input;
using GeoService_RESTLayer.Input_Output_Modellen.Output;
using GeoService_RESTLayer.Mappers.ExceptionsForMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Mappers {
    public class MapFromDomain {
        public static ContinentRESTOutputDTO MapFromContinentDomain(string url, Continent continent, CountryService countryservice)
        {
            try
            {
                string continentURL = $"{url}/api/Continent/{continent.Id}";
                int PopulationAmount = 0;
                var countryList = continent.GetCountries();
                foreach (var c in countryList)
                {
                    PopulationAmount += c.Population;
                }
                List<string> countries = countryList.Select(c => continentURL + $"/Country/{c.Id}").ToList();
                ContinentRESTOutputDTO DTO_Object = new(continentURL, continent.Name, PopulationAmount, countries.Count, countries);
                return DTO_Object;
            }
            catch (Exception ex)
            {
                throw new MapFromDomainException("MapFromDomain - MapFromContinentDomain " + ex.Message);
            }
        }

        public static CountryRESTOutputDTO MapFromCountryDomain(string url, Country country, CityService cityService)
        {
            try
            {
                string continentURL = $"{url}/api/Continent/{country.Continent.Id}";
                string countryURL = $"{continentURL}/Country/{country.Id}";
                List<string> cities = cityService.GeefStedenLand(country.Id).Select(x => countryURL + $"/City/{x.Id}").ToList();
                List<string> capitals = cityService.GeefStedenLand(country.Id)
                                        .Where(cp => cp.IsCapital)
                                        .Select(x => countryURL + $"/City/{x.Id}")
                                        .ToList();
                CountryRESTOutputDTO DTO_Object = new(countryURL, country.Name, country.Population, country.Surface, continentURL, capitals, cities);
                return DTO_Object;
            }
            catch (Exception ex)
            {
                throw new MapFromDomainException("MapFromDomain - MapFromCountryDomain ", ex);
            }
        }

        public static CityRESTOuputDTO MapFromCityDomain(string url, City city)
        {
            try
            {

                string continentURL = $"{url}/api/Continent/{city.Country.Continent.Id}";
                string landURL = $"{continentURL}/Country/{city.Country.Id}";
                string stadURL = $"{landURL}/City/{city.Id}";
                CityRESTOuputDTO DTO_Object = new(stadURL, city.Name, city.Population, city.IsCapital, continentURL, landURL);
                return DTO_Object;
            }
            catch (Exception ex)
            {
                throw new MapFromDomainException("MapFromDomain - MapFromCityDomain", ex);
            }
        }

        public static RiverRESTOutputDTO MapFromRiverDomain(string url, River river, RiverService riverService)
        {
            try
            {
                string riverURL = $"{url}/api/River/{river.Id}";
                List<Country> ListCountries = riverService.geefLandenRivier(river.Id);
                List<string> countries = new();
                foreach (var item in ListCountries)
                {
                    countries.Add($"{url}/api/Continent/{item.Continent.Id}/Country/{item.Id}");
                }
                RiverRESTOutputDTO dto = new(riverURL, river.Name, river.Length, countries);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapFromDomainException("MapFromDomain - MapFromRiverDomain ", ex);
            }
        }
    }
}
