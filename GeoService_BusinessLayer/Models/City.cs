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
        }
        public City(string name, int population, bool isCapital, Country country)
        {
            ZetNaam(name);
            ZetBevolkingsaantal(population);
            ZetLand(country);
            ZetIsHoofdstad(isCapital);
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
            bool oldValue = IsCapital;
            IsCapital = isCapital;
            if (isCapital == true && oldValue == false)
            {
                Country.ZetHoofdstad(this);
            }
            else if (isCapital == false && oldValue == true)
            {
                Country.RemoveAsCapital(this);
            }
        }
        public void ZetLand(Country country)
        {
            if (country == null)
                throw new CityException("City: City must belong to a country!");
            else
            {
                Country oldCountry = Country;
                Country = country;
                if (oldCountry != null)
                {
                    oldCountry.RemoveCity(this);
                }
                Country.ZetStad(this);
                if (IsCapital)
                    Country.ZetHoofdstad(this);
            }
        }
    }
}
