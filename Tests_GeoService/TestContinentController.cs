using DomeinLaag.Interfaces;
using DomeinLaag.Services;
using GeoService_BusinessLayer.Interfaces;
using GeoService_BusinessLayer.ModelExceptions;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.Services;
using GeoService_RESTLayer.Controllers;
using GeoService_RESTLayer.Input_Output_Modellen.Input;
using GeoService_RESTLayer.Input_Output_Modellen.Output;
using GeoService_RESTLayer.LogHandler;
using GeoService_RESTLayer.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests_GeoService.Tests_Controller {
    public class TestContinentController {
        private readonly GeoService_Controller geoService_Controller;
        private readonly Mock<ContinentService> _continentService;
        private readonly Mock<CountryService> _countryService;
        private readonly Mock<CityService> _cityService;
        private readonly Mock<RiverService> _riverService;
        private readonly Mock<ILogger<GeoService_Controller>> _logRequestAndResponseHandler;

        public TestContinentController()
        {
            _continentService = new Mock<ContinentService>();
            _countryService = new Mock<CountryService>();
            _cityService = new Mock<CityService>();
            _riverService = new Mock<RiverService>();
            _logRequestAndResponseHandler = new Mock<ILogger<GeoService_Controller>>();
            geoService_Controller = new GeoService_Controller(_continentService.Object, _cityService.Object, _countryService.Object, _riverService.Object, _logRequestAndResponseHandler.Object);
        }

        [Fact]
        public void Test_GETContinent_NotFound()
        {
            int wrongContinent = 1234;
            _continentService.Setup(repo => repo.ContinentWeergeven(wrongContinent)).Throws(new ContinentException("Continent bestaat niet"));
            var result = geoService_Controller.GETContinent(wrongContinent);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void Test_GETContinent_Correct()
        {
            int correctContinent = 1;
            _continentService.Setup(repo => repo.ContinentWeergeven(correctContinent)).Throws(new ContinentException("Continent bestaat"));
            var result = geoService_Controller.GETContinent(correctContinent);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
