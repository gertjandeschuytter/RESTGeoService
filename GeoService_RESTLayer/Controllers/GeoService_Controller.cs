using DomeinLaag.Interfaces;
using DomeinLaag.Services;
using GeoService_BusinessLayer.Interfaces;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.Services;
using GeoService_RESTLayer.Input_Output_Modellen.Input;
using GeoService_RESTLayer.Input_Output_Modellen.Output;
using GeoService_RESTLayer.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.Controllers {
    [ApiController]
    [Route("/api")]
    public class GeoService_Controller : ControllerBase {
        private readonly string url = "http://localhost:6385";


        private ContinentService continentService;
        private CityService cityService;
        private CountryService countryService;
        private RiverService riverService;
        private ILogger<GeoService_Controller> _loghandler;

        //public GeoService_Controller(ContinentService continentService, CityService cityService, CountryService countryService, RiverService riverService, ILogger<GeoService_Controller> loghandler)
        //{
        //    this.continentService = continentService;
        //    this.cityService = cityService;
        //    this.countryService = countryService;
        //    this.riverService = riverService;
        //    this._loghandler = loghandler;
        //}
        public GeoService_Controller(IContinentRepository continentrepo, ICityRepository cityRepo, ICountryRepository countryRepo, IRiverRepository riverRepo, ILogger<GeoService_Controller> loghandler)
        {
            this.continentService = new(continentrepo);
            this.cityService = new(cityRepo);
            this.countryService = new(countryRepo);
            this.riverService = new(riverRepo);
            this._loghandler = loghandler;
        }
        //Continent ==> IN ORDE 
        [HttpGet]
        [Route("Continent/{continentId}")]
        public ActionResult<ContinentRESTOutputDTO> GETContinent(int continentId)
        {
            _loghandler.LogInformation($"GETContinent was called.");
            try
            {
                Continent continent = continentService.ContinentWeergeven(continentId);
                _loghandler.LogInformation($"Continent met (ID: {continentId}) werd succesvol opgevraagd");
                return Ok(MapFromDomain.MapFromContinentDomain(url, continent));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"GETContinent failed " + ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Route("Continent/")]
        public ActionResult<ContinentRESTOutputDTO> POSTContinent([FromBody] ContinentRESTInputDTO dtoObject)
        {
            _loghandler.LogInformation($"POSTContinent was called.");
            try
            {
                Continent continent = continentService.ContinentToevoegen(MapToDomain.MapToContinentDomain(dtoObject));
                _loghandler.LogInformation($"POSTContinent was succesful");
                return CreatedAtAction(nameof(GETContinent), new { continentId = continent.Id }, MapFromDomain.MapFromContinentDomain(url, continent));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"POSTContinent - failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("Continent/{continentId}")]
        public ActionResult DELETEContinent(int continentId)
        {
            _loghandler.LogInformation($"DELETEContinent was called.");
            try
            {
                continentService.ContinentVerwijderen(continentId);
                _loghandler.LogInformation($"Continent was succesfully removed");
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"DELETEContinent failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Continent/{continentId}")]
        public ActionResult<ContinentRESTOutputDTO> PUTContinent(int continentId, [FromBody] ContinentRESTInputDTO dtoObject)
        {
            _loghandler.LogInformation($"PUTContinent was called.");
            try
            {
                Continent continent = MapToDomain.MapToContinentDomain(dtoObject);
                continent.ZetId(continentId);
                continent = continentService.ContinentUpdaten(continent);
                _loghandler.LogInformation($"PUTContinent was succesful.");
                return CreatedAtAction(nameof(GETContinent), new { continentId = continent.Id }, MapFromDomain.MapFromContinentDomain(url, continent));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"PUTContinent failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------->

        //Countries ==> IN ORDE  
        [HttpGet]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult<CountryRESTOutputDTO> GETCountry(int continentId, int countryId)
        {
            _loghandler.LogInformation($"GETCountry was called.");
            try
            {
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The continent with the given Id does not exist.");
                    return BadRequest($"The continent with the given Id does not exist.");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogInformation($"Country is not in the given continent.");
                    return BadRequest($"Country is not in the given continent");
                }
                Country country = countryService.LandWeergeven(countryId);
                _loghandler.LogInformation($"GETCountry was succesful.");
                return Ok(MapFromDomain.MapFromCountryDomain(url, country, cityService, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"GETCountry failed " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("Continent/{continentId}/Country")]
        public ActionResult<CountryRESTOutputDTO> POSTCountry(int continentId, [FromBody] CountryRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"POSTCountry was called.");
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogInformation($"POSTCountry - The filled in continentId does not match the id given in the DTO.");
                    return BadRequest("The filled in continentId does not match the id given in the DTO.");
                }
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"POSTCountry - The filled in continentId does not match the id given in the DTO.");
                    return BadRequest("The filled in continentId does not match the id given in the DTO.");
                }
                Country country = countryService.LandToevoegen(MapToDomain.MapToCountryDomain(dtoObject, continentService));
                _loghandler.LogInformation($"POSTCountry was succesful.");
                return CreatedAtAction(nameof(GETCountry), new { continentId, countryId = country.Id }, MapFromDomain.MapFromCountryDomain(url, country, cityService, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"POSTCountry failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult DELETECountry(int continentId, int countryId)
        {
            try
            {
                _loghandler.LogInformation($"DELETECountry was called.");
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The continent does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"The country does not exist.");
                    return BadRequest($"The country does not exist.");
                }
                if (cityService.HeeftSteden(countryId))
                {
                    _loghandler.LogInformation($"The given country cannot be removed because it still contains cities.");
                    return BadRequest($"The given country cannot be removed because it still contains cities.");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogInformation($"Country is not in the given continent.");
                    return BadRequest($"Country is not in the given continent.");
                }
                countryService.LandVerwijderen(countryId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"DELETECountry failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult<ContinentRESTOutputDTO> PUTCountry(int continentId, int countryId, [FromBody] CountryRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"PUTCountry was called.");
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogInformation($"The given countryId do not match each other.");
                    return BadRequest($"The given countryId do not match each other.");
                }
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogInformation($"The give continentId do not match each other.");
                    return BadRequest($"The give continentId do not match each other.");
                }
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The given continentid does not exist.");
                    return BadRequest($"Het continent/land bestaat al of ingevulde informatie is leeg/null.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"The given countryId does not exist.");
                    return BadRequest($"The given countryId does not exist.");
                }
                if (dtoObject == null)
                {
                    _loghandler.LogInformation($"The given object is null");
                    return BadRequest($"The given object is null");
                }
                if (string.IsNullOrWhiteSpace(dtoObject.Name))
                {
                    _loghandler.LogInformation($"The given name is null or contains whitespace");
                    return BadRequest($"The given name is null or contains whitespace");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogInformation($"Given country is not in the given Continent.");
                    return BadRequest($"Given country is not in the given continent.");
                }
                Country country = MapToDomain.MapToCountryDomain(dtoObject, continentService);
                country.ZetId(countryId);
                country = countryService.LandUpdaten(country);
                _loghandler.LogInformation($"PutLand methode werdt succesvol opgeroepen.");
                return CreatedAtAction(nameof(GETCountry), new { continentId, countryId = country.Id }, MapFromDomain.MapFromCountryDomain(url, country, cityService, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"PUTCountry failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------->

        //Cities ==> IN ORDE 
        [HttpGet]
        [Route("Continent/{continentId}/Country/{countryId}/City/{cityId}")]
        public ActionResult<CityRESTOuputDTO> GETCity(int continentId, int countryId, int cityId)
        {
            try
            {
                _loghandler.LogInformation($"GETCity was called.");
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The continent does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"The country does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!cityService.BestaatStad(cityId))
                {
                    _loghandler.LogInformation($"The city does not exist.");
                    return BadRequest($"The city does not exist.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogInformation($"The combination of ids does not match each other");
                    return BadRequest($"The combination of ids does not match each other.");
                }
                City city = cityService.StadWeergeven(cityId);
                _loghandler.LogInformation($"GETCity was succesful");
                return Ok(MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"DATE: {DateTime.Now} - MESSAGE: GETCity failed " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("Continent/{continentId}/Country/{countryId}/City")]
        public ActionResult<CityRESTOuputDTO> POSTCity(int continentId, int countryId, [FromBody] CityRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"POSTCity was called.");
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogInformation($"The continentId is not equal to the given continentId.");
                    return BadRequest($"The continentId is not equal to the given continentId.");
                }
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogInformation($"The countryId is not equal to the given countryId.");
                    return BadRequest($"The countryId is not equal to the given countryId.");
                }
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The Continent does not exist.");
                    return BadRequest($"The Continent does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"The Country does not exist.");
                    return BadRequest($"The Country does not exist.");
                }
                City city = cityService.StadToevoegen(MapToDomain.MapToCityDomain(dtoObject, continentService, countryService));
                _loghandler.LogInformation($"POSTCity was succesful.");
                return CreatedAtAction(nameof(GETCity), new { continentId, countryId, cityId = city.Id }, MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"POSTCity failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Continent/{continentId}/Country/{countryId}/Stad/{cityId}")]
        public ActionResult DELETECity(int continentId, int countryId, int cityId)
        {
            try
            {
                _loghandler.LogInformation($"DELETECity was called.");
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"A continent with the given Id does not exist.");
                    return BadRequest($"A continent with the given Id does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"A country with the given Id does not exist.");
                    return BadRequest($"A country with the given Id does not exist.");
                }
                if (!cityService.BestaatStad(cityId))
                {
                    _loghandler.LogInformation($"A city with the given Id does not exist.");
                    return BadRequest($"A city with the given Id does not exist.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogInformation($"The combination of ids does not match each other");
                    return BadRequest($"The combination of ids does not match each other.");
                }
                cityService.StadVerwijderen(cityId);
                _loghandler.LogInformation($"DELETECity was succesful");
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"DELETECity failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Continent/{continentId}/Country/{countryId}/City/{cityId}")]
        public ActionResult<CityRESTOuputDTO> PUTCity(int continentId, int countryId, int cityId, [FromBody] CityRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"PUTCity was called.");
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogInformation($"The continent/country/city does not exist.");
                    return BadRequest($"A continent with the given Id does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogInformation($"The country does not exist.");
                    return BadRequest($"A country with the given Id does not exist.");
                }
                if (!cityService.BestaatStad(cityId))
                {
                    _loghandler.LogInformation($"MESSAGE: The city does not exist.");
                    return BadRequest($"A city with the given Id does not exist.");
                }
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogInformation($"The continentId and the continentId within the object do not match.");
                    return BadRequest($"The continentId and the continentId within the object do not match.");
                }
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogInformation($"The countryId and the countryId within the object do not match.");
                    return BadRequest($"The countryId and the countryId within the object do not match.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogInformation($"The combination of ids of city, country and continent do not match.");
                    return BadRequest($"The combination of ids of city, country and continent do not match.");
                }
                City city = MapToDomain.MapToCityDomain(dtoObject, continentService, countryService);
                city.ZetId(cityId);
                City stadDb = cityService.StadUpdaten(city);
                _loghandler.LogInformation($"PUTCity was succesful");
                return CreatedAtAction(nameof(GETCity), new { continentId, countryId, cityId = city.Id }, MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"PUTCity failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------->

        [HttpGet]
        [Route("River/{riverId}")]
        public ActionResult<RiverRESTOutputDTO> GETRiver(int riverId)
        {
            try
            {
                _loghandler.LogInformation($"GETRiver was called.");
                River river = riverService.RivierWeergeven(riverId);
                river.ZetId(riverId);
                _loghandler.LogInformation($"Rivier with {riverId} was succesfuly returned");
                return Ok(MapFromDomain.MapFromRiverDomain(url, river, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"GETRiver - failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("River/")]
        public ActionResult<RiverRESTOutputDTO> POSTRiver([FromBody] RiverRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"POSTRiver was called.");
                River river = riverService.RivierToevoegen(MapToDomain.MapToRiverDomain(dtoObject, countryService));
                _loghandler.LogInformation($"POSTRiver was succesful");
                return CreatedAtAction(nameof(GETRiver), new { riverId = river.Id }, MapFromDomain.MapFromRiverDomain(url, river, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"POSTRiver - failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("River/{riverId}")]
        public ActionResult DELETERiver(int riverId)
        {
            try
            {
                _loghandler.LogInformation($"DELETERiver was called.");
                riverService.RivierVerwijderen(riverId);
                _loghandler.LogInformation($"River {riverId} was succesfully removed");
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"DELETERiver failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("River/{riverId}")]
        public ActionResult<RiverRESTOutputDTO> PUTRiver(int riverId, [FromBody] RiverRESTInputDTO dtoObject)
        {
            try
            {
                _loghandler.LogInformation($"PUTRiver was called.");
                if (dtoObject.RiverId != riverId)
                {
                    _loghandler.LogInformation($"The Id does not match the Id in the paramaters.");
                    return BadRequest($"The Id does not match the Id in the paramaters.");
                }
                River river = MapToDomain.MapToRiverDomain(dtoObject, countryService);
                river.ZetId(riverId);
                riverService.UpdateRiver(river);
                _loghandler.LogInformation($"PUTRiver was succesful.");
                return CreatedAtAction(nameof(GETRiver), new { riverId = river.Id }, MapFromDomain.MapFromRiverDomain(url, river, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogInformation($"PUTRiver failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------->

    }
}
