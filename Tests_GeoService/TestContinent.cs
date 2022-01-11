using GeoService_BusinessLayer.ModelExceptions;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Businesslayer_Tests.Tests_BusinessLayer {
    public class TestContinent {
        //[Fact]
        //public void Test_ContinentNameEmpty_ThrowException()
        //{
        //    Assert.Throws<ContinentException>(() => new Continent(""));
        //}

        //[Fact]
        //public void Test_ContinentNameNull_ThrowException()
        //{
        //    Assert.Throws<ContinentException>(() => new Continent(null));
        //}

        //[Fact]
        //public void Test_FullContinent_Succes()
        //{
        //    Continent c = new Continent("Europe");
        //    List<Country> countries = new List<Country>();
        //    Country country1 = new Country("Belgium", 10000000, 35000, c);
        //    Country country2 = new Country("Germany", 21000000, 95000, c);
        //    countries.Add(country1);
        //    countries.Add(country2);
        //    c.ZetLanden(countries);
        //    Assert.True(c.GetCountries().Count == 2, "Het aantal landen was niet correct");
        //}

        //[Fact]
        //public void Test_ApplyPopulation_Succes()
        //{
        //    int bevolkingsaantal_1 = 200000;
        //    Continent c = new Continent("South-America");
        //    Assert.True(c.GetPopulation() == 0, "Population should be equal to 0");
        //    Country country1 = new Country("Brazil", bevolkingsaantal_1, 150000, c);
        //    Assert.True(c.GetPopulation() == bevolkingsaantal_1, "The population did not get updated");
        //    int bevolkingsaantal_2 = 500000;
        //    Country country2 = new Country("Uruguay", bevolkingsaantal_2, 125000, c);
        //    c.RemoveCountry(country1);
        //    Assert.True(c.GetPopulation() == bevolkingsaantal_2);
        //}
    }
}
