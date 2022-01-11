using DomeinLaag.Interfaces;
using DomeinLaag.Services;
using GeoService_BusinessLayer.Interfaces;
using GeoService_BusinessLayer.ModelExceptions;
using GeoService_BusinessLayer.Models;
using GeoService_BusinessLayer.Services;
using GeoService_RESTLayer.Controllers;
using GeoService_RESTLayer.Input_Output_Modellen.Input;
using GeoService_RESTLayer.Input_Output_Modellen.Output;
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

        private readonly Mock<IContinentRepository> _continentRepo;
        private readonly Mock<ICountryRepository> _countryRepo;
        private readonly Mock<ICityRepository> _cityRepo;
        private readonly Mock<IRiverRepository> _riverRepo;
        private readonly Mock<ILogger<GeoService_Controller>> MockLogger;
        private readonly GeoService_Controller geoService_Controller;

        public ContinentService continentService { get; set; }


        public TestContinentController()
        {
            _continentRepo = new();
            _countryRepo = new();
            _cityRepo = new();
            _riverRepo = new();
            MockLogger = new();
            geoService_Controller = new(_continentRepo.Object, _cityRepo.Object, _countryRepo.Object, _riverRepo.Object, MockLogger.Object);
        }

        //[Fact]
        //public void Test_GETContinent_NotFound()
        //{
        //    int wrongContinent = 1234;
        //    _continentRepo.Setup(repo => repo.ContinentWeergevenMetLanden(wrongContinent)).Throws(new ContinentException("Continent bestaat niet"));
        //    var result = geoService_Controller.GETContinent(wrongContinent);
        //    Assert.IsType<NotFoundObjectResult>(result.Result);
        //}

        //lukt niet
        [Fact]
        public void Test_GETContinent_ReturnsOk()
        {
            int correctContinent = 1;
            _continentRepo.Setup(repo => repo.ContinentWeergevenMetLanden(correctContinent)).Returns(new Continent("Europa"));
            var result = geoService_Controller.GETContinent(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }


    }
}
