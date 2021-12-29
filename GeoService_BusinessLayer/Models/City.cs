using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models {
    public class City {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Population { get; private set; }
        public bool IsCapital { get; private set; }
        public Country Country { get; private set; }

        public City(int id, string name, int population, bool isCapital, Country country) : this(name,population,isCapital,country)
        {
            ZetId(id);
            ZetNaam(name);
            ZetBevolkingsaantal(population);
            ZetIsHoofdstad(isCapital);
            ZetLand(country);
        }
        public City(string name, int population, bool isCapital, Country country)
        {
            ZetNaam(name);
            ZetBevolkingsaantal(population);
            ZetIsHoofdstad(isCapital);
            ZetLand(country);
        }

        //setters
        public void ZetId(int id)
        {
            if (id <= 0)
            {
                throw new CityException("Id moet groter zijn dan 0.");
            }
            Id = id;
        }
        public void ZetNaam(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CityException("Naam mag niet leeg zijn.");
            }
            Name = name;
        }
        public void ZetBevolkingsaantal(int population)
        {
            if (population <= 0)
            {
                throw new CityException("Bevolkingsaantal kan niet kleiner of gelijk zijn dan 0.");
            }
            Population = population;
        }
        public void ZetIsHoofdstad(bool isCapital)
        {
            IsCapital = isCapital;
        }
        public void ZetLand(Country country)
        {
            if (country == null)
            {
                throw new CityException("Land kan niet null zijn.");
            }
            Country = country;
        }
    }
}
