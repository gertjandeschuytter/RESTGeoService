using DomeinLaag.Services;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.Services;
using GeoService_RESTLayer.Input_Output_Modellen.Input;
using GeoService_RESTLayer.Input_Output_Modellen.Output;
using GeoService_RESTLayer.LogHandler;
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
        private ContinentService continentService;
        private CityService cityService;
        private CountryService countryService;
        private RiverService riverService;
        private LogRequestAndResponseHandler _loghandler;
        private string url = "http://localhost:6385";

        public GeoService_Controller(ContinentService continentService, CityService cityService, CountryService countryService, RiverService riverService, LogRequestAndResponseHandler loghandler)
        {
            this.continentService = continentService;
            this.cityService = cityService;
            this.countryService = countryService;
            this.riverService = riverService;
            _loghandler = loghandler;
        }
        //Continent
        [HttpGet]
        [Route("Continent/{continentId}")]
        public ActionResult<ContinentRESTOutputDTO> GETContinent(int continentId)
        {
            try
            {
                Continent continent = continentService.ContinentWeergeven(continentId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Continent met (ID: {continentId}) werd succesvol opgevraagd");
                return Ok(MapFromDomain.MapFromContinentDomain(url, continent, countryService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: GetContinent met meegegeven (ID: {continentId}) kan niet worden opgevraagd " + ex.Message);
                return NotFound(ex);
            }
        }
        [HttpPost]
        [Route("Continent/")]
        public ActionResult<ContinentRESTOutputDTO> POSTContinent([FromBody] ContinentRESTInputDTO dtoObject)
        {
            try
            {
                Continent continent = continentService.ContinentToevoegen(MapToDomain.MapToContinentDomain(dtoObject));
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POST Continent was succesvol");
                return CreatedAtAction(nameof(GETContinent), new { continentId = continent.Id }, MapFromDomain.MapFromContinentDomain(url, continent, countryService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POST Continent - kon niet worden aangemaakt " + ex.Message);
                return BadRequest(ex);
            }
        }
        [HttpDelete]
        [Route("Continent/{continentId}")]
        public ActionResult DELETEContinent(int continentId)
        {
            try
            {
                continentService.ContinentVerwijderen(continentId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Continent was succesfully removed");
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: DeleteContinent failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Continent/{continentId}")]
        public ActionResult<ContinentRESTOutputDTO> PUTContinent(int continentId, [FromBody] ContinentRESTInputDTO dtoObject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dtoObject.Name) || dtoObject == null)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continent to be updated contains whitespaces or is null.");
                    return BadRequest($"The continent to be updated contains whitespaces or is null.");
                }
                Continent continent = MapToDomain.MapToContinentDomain(dtoObject);
                continent.ZetId(continentId);
                continentService.ContinentUpdaten(continent);
                continent = continentService.ContinentWeergeven(continentId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE:  PutContinent was succesful.");
                return CreatedAtAction(nameof(GETContinent), new { continentId = continent.Id }, MapFromDomain.MapFromContinentDomain(url, continent, countryService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: PutContinent failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        //countries
        [HttpGet]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult<CountryRESTOutputDTO> GETCountry(int continentId, int countryId)
        {
            try
            {
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continent with the given Id does not exist.");
                    return BadRequest($"The continent with the given Id does not exist.");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Country is not in the given continent.");
                    return BadRequest($"Country is not in the given continent");
                }
                Country country = countryService.LandWeergeven(countryId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE:  GetCountry was succesful.");
                return Ok(MapFromDomain.MapFromCountryDomain(url, country, cityService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: GetCountry failed " + ex.Message);
                return NotFound(ex);
            }
        }

        [HttpPost]
        [Route("Continent/{continentId}/Country")]
        public ActionResult<CountryRESTOutputDTO> POSTCountry(int continentId, [FromBody] CountryRESTInputDTO dtoObject)
        {
            try
            {
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE:  The filled in continentId does not match the id given in the DTO.");
                    return BadRequest("The filled in continentId does not match the id given in the DTO.");
                }
                //voeg een transactie toe in landtoevoegen om ook ondertussen het continent up te daten
                Country country = countryService.LandToevoegen(MapToDomain.MapToCountryDomain(dtoObject, continentService));
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE:  POSTCountry was succesful.");
                return CreatedAtAction(nameof(GETCountry), new { continentId, countryId = country.Id }, MapFromDomain.MapFromCountryDomain(url, country, cityService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POSTCountry failed " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult DELETECountry(int continentId, int countryId)
        {
            Country c = null;
            try
            {
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continent does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The country does not exist.");
                    return BadRequest($"The country does not exist.");
                }
                c = countryService.LandWeergeven(countryId);
                if (cityService.HeeftSteden(countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE:  The given country cannot be removed because it still contains cities.");
                    return BadRequest($"The given country cannot be removed because it still contains cities.");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Country is not in the given continent.");
                    return BadRequest($"Country is not in the given continent.");
                }
                countryService.LandVerwijderen(countryId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: DELETECountry failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("Continent/{continentId}/Country/{countryId}")]
        public ActionResult<ContinentRESTOutputDTO> PUTCountry(int continentId, int countryId, [FromBody] CountryRESTInputDTO dtoObject)
        {
            try
            {
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The give continentId and countryId do not match each other.");
                    return BadRequest($"The give continentId and countryId do not match each other.");
                }
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The given continentid does not exist.");
                    return BadRequest($"Het continent/land bestaat al of ingevulde informatie is leeg/null.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The given countryId does not exist.");
                    return BadRequest($"The given countryId does not exist.");
                }
                if (dtoObject == null)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The given object is null");
                    return BadRequest($"The given object is null");
                }
                if (string.IsNullOrWhiteSpace(dtoObject.Name))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The given name is null or contains whitespace");
                    return BadRequest($"The given name is null or contains whitespace");
                }
                if (!countryService.ZitLandInContinent(continentId, countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Given country is not in the given Continent.");
                    return BadRequest($"Given country is not in the given continent.");
                }
                Country country = MapToDomain.MapToCountryDomain(dtoObject, continentService);
                country.ZetId(countryId);
                countryService.LandUpdaten(country);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: PutLand methode werdt succesvol opgeroepen.");
                return CreatedAtAction(nameof(GETCountry), new { continentId, countryId = country.Id }, MapFromDomain.MapFromCountryDomain(url, country, cityService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: PUTCountry failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        //cities
        [HttpGet]
        [Route("Continent/{continentId}/Country/{countryId}/City/{cityId}")]
        public ActionResult<CityRESTOuputDTO> GETCity(int continentId, int countryId, int cityId)
        {
            try
            {
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continent does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The country does not exist.");
                    return BadRequest($"The continent does not exist.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - The combination of ids does not match each other");
                    return BadRequest($"The combination of ids does not match each other.");
                }
                City city = cityService.StadWeergeven(cityId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - GETCity was succesfull");
                return Ok(MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: GETCity failed " + ex.Message);
                return NotFound(ex);
            }
        }

        [HttpPost]
        [Route("Continent/{continentId}/Country/{countryId}/City")]
        public ActionResult<CityRESTOuputDTO> POSTCity(int continentId, int countryId, [FromBody] CityRESTInputDTO dtoObject)
        {
            try
            {
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continentId is not equal to the given continentId.");
                    return BadRequest($"The continentId is not equal to the given continentId.");
                }
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The countryId is not equal to the given countryId.");
                    return BadRequest($"The countryId is not equal to the given countryId.");
                }
                if (!cityService.ControleerBevolkingsaantal(countryId, dtoObject.Population))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The given population is bigger than the population of the country.");
                    return BadRequest("The given population is bigger than the population of the country.");
                }
                City city = cityService.StadToevoegen(MapToDomain.MapToCityDomain(dtoObject, continentService, countryService));
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POSTCity was succesful.");
                return CreatedAtAction(nameof(GETCity), new { continentId, countryId, cityId = city.Id }, MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POSTCity failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("Continent/{continentId}/Country/{countryId}/Stad/{cityId}")]
        public ActionResult DELETECity(int continentId, int countryId, int cityId)
        {
            try
            {
                if (!continentService.BestaatContinent(continentId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: A continent with the given Id does not exist.");
                    return BadRequest($"A continent with the given Id does not exist.");
                }
                if (!countryService.BestaatLand(countryId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: A country with the given Id does not exist.");
                    return BadRequest($"A country with the given Id does not exist.");
                }
                if (!cityService.BestaatStad(cityId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: A city with the given Id does not exist.");
                    return BadRequest($"A city with the given Id does not exist.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - The combination of ids does not match each other");
                    return BadRequest($"The combination of ids does not match each other.");
                }
                cityService.StadVerwijderen(cityId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - DELETECity was succesful");
                return NoContent();
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: DELETECity failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("Continent/{continentId}/Country/{countryId}/City/{cityId}")]
        public ActionResult<CityRESTOuputDTO> PUTCity(int continentId, int countryId, int cityId, [FromBody] CityRESTInputDTO dtoObject)
        {
            try
            {
                if (!continentService.BestaatContinent(continentId) || !countryService.BestaatLand(countryId) || !cityService.BestaatStad(cityId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continent/country/city does not exist.");
                    return BadRequest($"A continent with the given Id does not exist.");
                }
                if (dtoObject == null || string.IsNullOrWhiteSpace(dtoObject.Name))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The new city name contains whitespaces or is null.");
                    return BadRequest($"The new city name contains whitespaces or is null.");
                }
                if (continentId != dtoObject.ContinentId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The continentId and the continentId within the object do not match.");
                    return BadRequest($"The continentId and the continentId within the object do not match.");
                }
                if (countryId != dtoObject.CountryId)
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The countryId and the countryId within the object do not match.");
                    return BadRequest($"The countryId and the countryId within the object do not match.");
                }
                if (!cityService.ZitStadInLandInContinent(continentId, countryId, cityId))
                {
                    _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: The combination of ids of city, country and continent do not match.");
                    return BadRequest($"The combination of ids of city, country and continent do not match.");
                }
                City city = MapToDomain.MapToCityDomain(dtoObject, continentService, countryService);
                city.ZetId(cityId);
                //als meegegeven populatie kleiner is dan de huidige populatie van een stad is er geen controle nodig
                if (dtoObject.Population! < city.Population)
                {
                    if (!cityService.ControleerBevolkingsaantal(countryId, dtoObject.Population))
                    {
                        _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Updated population exceeds the population of the country, please pick a lower number.");
                        return BadRequest($"Updated population exceeds the population of the country, please pick a lower number.");
                    }
                }
                City stadDb = cityService.StadUpdaten(city);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: PUTCity was succesful");
                return CreatedAtAction(nameof(GETCity), new { continentId, countryId, cityId = city.Id }, MapFromDomain.MapFromCityDomain(url, city));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: PUTCity failed " + ex.Message);
                return BadRequest(ex);
            }
        }

        //river
        [HttpGet]
        [Route("River/{riverId}")]
        public ActionResult<RiverRESTOutputDTO> GetRiver(int riverId)
        {
            try
            {
                River river = riverService.RivierWeergeven(riverId);
                river.ZetId(riverId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Rivier met (ID: {riverId}) werd succesvol opgevraagd");
                return Ok(MapFromDomain.MapFromRiverDomain(url, river, riverService));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("River/")]
        public ActionResult<RiverRESTOutputDTO> PostRiver([FromBody] RiverRESTInputDTO dtoObject)
        {
            try
            {
                River river = riverService.RivierToevoegen(MapToDomain.MapToRiverDomain(dtoObject, countryService));
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POST Continent was succesvol");
                return CreatedAtAction(nameof(GetRiver), new { riverId = river.Id }, MapFromDomain.MapFromRiverDomain(url, river, riverService));
            }
            catch (Exception ex)
            {
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: POST Continent - kon niet worden aangemaakt " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("River/{riverId}")]
        public ActionResult DeleteRiver(int riverId)
        {
            try
            {
                riverService.RivierVerwijderen(riverId);
                _loghandler.LogRequestOrResponse($"DATE: {DateTime.Now} - MESSAGE: Continent was succesfully removed");
                return NoContent();
            }
            catch (Exception) { return BadRequest(); }
        }
        //[HttpPut]
        //[Route("{id}")]
        //public ActionResult<RiverDTOutput> PutRiver(int id, [FromBody] RiverDTOInput river)
        //{
        //    Logger.LogInformation("PutRiver called");
        //    if (river == null || river.RiverId != id)
        //    {
        //        return BadRequest("The id for the river was not in good condition");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            return CreatedAtAction(nameof(PutRiver), ApiComplete.UpdateRiver(river));
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }
        //}
    }
}
