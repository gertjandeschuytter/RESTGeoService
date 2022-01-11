using GeoService_BusinessLayer.ModelExceptions;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Businesslayer_Tests.Tests_BusinessLayer {
    public class TestCity {
        //private City GetStandardCity()
        //{
        //    string cityName = "testCity";
        //    int population = 35000;
        //    Continent continent = new Continent("testContinent");
        //    Country country = new Country("testCountry", 40000, 10, continent);
        //    City city = new City(cityName, population, true, country);

        //    return city;
        //}

        //private Country GetStandardCountry()
        //{
        //    Continent continent = new Continent("testContinent");
        //    Country country = new Country("testCountry", 40000, 10, continent);
        //    return country;
        //}

        //[Fact]
        //private void Test_City_Data_Succeed()
        //{
        //    string name = "Varsenare";
        //    int population = 35000;
        //    Continent c = new Continent("Europe");
        //    Country ctry = new Country("Belgium", 40000, 10, c);
        //    City city = new City(name, population, true, ctry);

        //    Assert.True(city.Name == name, "The name of the city was not correct");
        //    Assert.True(city.Population == population, "The population of the city was not correct");
        //    Assert.True(city.Country.Equals(ctry), "The country of the city was not correct");
        //    Assert.True(ctry.GetCities().Contains(city), "The country did not contain the city");
        //    Assert.True(ctry.GetCapitals().Contains(city), "The country's capitals did not contain the city");
        //    Assert.True(city.IsCapital == true, "The city did not show it was a capital");
        //}

        //[Fact]
        //public void Test_PopulationLessThanZero_ShouldThrowException()
        //{
        //    Country country = GetStandardCountry();
        //    Assert.Throws<CityException>(() => new City("Jabbeke", -1, false, country));
        //}

        //[Fact]
        //public void Test_PopulationIsZero_ShouldThrowException()
        //{
        //    Country country = GetStandardCountry();
        //    Assert.Throws<CityException>(() => new City("Diksmuide", 0, false, country));
        //}

        //[Fact]
        //public void Test_CountryNullValue_ShouldThrowException()
        //{
        //    Assert.Throws<CityException>(() => new City("De Panne", 49000, false, null));
        //}
    }
}
