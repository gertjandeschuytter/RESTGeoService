using GeoService_BusinessLayer.ModelExceptions;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Businesslayer_Tests.Tests_BusinessLayer {
    public class TestRiver {
        //private Country GetFirstCountry()
        //{
        //    Continent continent = new Continent("testContinent");
        //    Country country = new Country("testCountry", 40000, 10, continent);
        //    return country;
        //}
        //private Country GetSecondCountry()
        //{
        //    Continent continent = new Continent("testContinent");
        //    Country country = new Country("testCountry2", 4000, 100, continent);
        //    return country;
        //}

        //[Fact]
        //public void Test_ShouldSetDataCorrectly()
        //{
        //    string name = "TestRiver";
        //    int length = 25;
        //    List<Country> countries = new List<Country>() { GetFirstCountry() };

        //    River river = new River(name, length, countries);

        //    Assert.True(river.Name == name, "The name of the river did not match.");
        //    Assert.True(river.Length == length, "The length of the river did not match.");
        //    Assert.True(river.GetCountries().SequenceEqual(countries), "The countries of the river did not match.");
        //}

        //[Fact]
        //public void Test_CreateRiverWithoutCountries()
        //{
        //    string name = "TestRiver";
        //    int length = 25;
        //    List<Country> countries = new List<Country>() { };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}

        //[Fact]
        //public void Test_CreateRiverWithDoublesInListOfCountries()
        //{
        //    string name = "TestRiver";
        //    int length = 25;
        //    List<Country> countries = new List<Country>() {
        //        GetFirstCountry(),
        //        GetFirstCountry() 
        //    };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}

        //[Fact]
        //public void Test_ChangeCountriesOfRiver_ListOfRiverInCountryNoLongerContainsRiver()
        //{
        //    string name1 = "TestRiver1";
        //    int length1 = 25;
        //    List<Country> countries1 = new List<Country>() { GetFirstCountry() };

        //    string name2 = "TestRiver2";
        //    int length2 = 225;
        //    List<Country> countries2 = new List<Country>() { GetSecondCountry() };

        //    River river1 = new River(name1, length1, countries1);
        //    River river2 = new River(name2, length2, countries2);
        //    river1.SetCountries(countries2);

        //    Assert.True(countries1[0].GetRivers().Count == 0, "The rivers were not correctly removed from the country.");
        //}

        //[Fact]
        //public void Test_WithNullAsName_ShouldThrowRiverException()
        //{
        //    string name = null;
        //    int length = 25;
        //    List<Country> countries = new List<Country>() { GetFirstCountry(), GetFirstCountry() };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}

        //[Fact]
        //public void Test_WithEmptyStringAsName_ShouldThrowRiverException()
        //{
        //    string name = "";
        //    int length = 25;
        //    List<Country> countries = new List<Country>() { GetFirstCountry(), GetFirstCountry() };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}

        //[Fact]
        //public void Test_With0AsLength_ShouldThrowRiverException()
        //{
        //    string name = "TestName";
        //    int length = 0;
        //    List<Country> countries = new List<Country>() { GetFirstCountry(), GetFirstCountry() };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}

        //[Fact]
        //public void Test_WithNegativeNumberAsLength_ShouldThrowRiverException()
        //{
        //    string name = "TestName";
        //    int length = -1;
        //    List<Country> countries = new List<Country>() { GetFirstCountry(), GetFirstCountry() };

        //    Assert.Throws<RiverException>(() => new River(name, length, countries));
        //}
    }
}
