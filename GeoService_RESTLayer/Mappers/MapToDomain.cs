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
    public class MapToDomain {
        public static Continent MapToContinentDomain(ContinentRESTInputDTO dtoObject)
        {
            try
            {
                if (dtoObject == null)
                {
                    throw new MapToDomainException("meegegeven object is null");
                }
                Continent continent = new(dtoObject.Name);
                return continent;
            }
            catch (Exception ex)
            {
                throw new MapToDomainException("MapToDomain - MapToContinentDomain " + ex.Message);
            }
        }

        public static Country MapToCountryDomain(CountryRESTInputDTO dtoObject, ContinentService continentService)
        {
            try
            {
                Continent continent = continentService.ContinentWeergeven(dtoObject.ContinentId);
                Country country = new(dtoObject.Name, dtoObject.Population, dtoObject.Surface, continent);
                continent.AddCountry(country);
                return country;
            }
            catch (Exception ex)
            {
                throw new MapToDomainException("MapToDomain - MapToCountryDomain ", ex);
            }
        }

        public static City MapToCityDomain(CityRESTInputDTO dto, ContinentService continentService, CountryService countryService)
        {
            try
            {
                Continent continent = continentService.ContinentWeergeven(dto.ContinentId);
                Country country = countryService.LandWeergeven(dto.CountryId);
                City city = new(dto.Name, dto.Population, dto.IsCapital, country);
                return city;
            }
            catch (Exception ex)
            {
                throw new MapToDomainException("MapNaarStadDomein - MapToCityDomain ", ex);
            }
        }

        public static River MapToRiverDomain(RiverRESTInputDTO dto, CountryService countryService)
        {
            try
            {
                River river = new(dto.Name, dto.Length);
                List<Country> countries = new List<Country>();
                foreach (var item in dto.CountryIds)
                {
                    countries.Add(countryService.LandWeergeven(item));
                }
                river.SetCountries(countries);
                return river;
            }
            catch (Exception ex)
            {
                throw new MapToDomainException("MapToDomain - MapToContinentDomain " + ex.Message);
            }
        }
    }
}
