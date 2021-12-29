using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models {
    public class Continent {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Population { get; private set; }
        private List<Country> _countries { get; set; } = new List<Country>();

        public Continent(string name, int id) : this(name)
        {
            ZetId(id);
            ZetNaam(name);
        }
        public Continent(string name)
        {
            ZetNaam(name);
        }
        public IReadOnlyList<Country> GetCountries()
        {
            return _countries.AsReadOnly();
        }
        public void AddCountry(Country country)
        {
            if (_countries.Contains(country))
            {
                throw new ContinentException("This country is already in this continent");
            }
            foreach (var item in _countries)
            {
                if (item.Name == country.Name)
                    throw new ContinentException("The name of a country in a continent must always be unique!");
            }
            _countries.Add(country);
        }
        //setters
        public void ZetId(int id)
        {
            if (id <= 0) throw new ContinentException("Id moet groter zijn dan 0.");
            Id = id;
        }
        public void ZetNaam(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ContinentException("Naam mag niet leeg zijn");
            Name = name;
        }
        public void ZetBevolkingsaantal(int population)
        {
            if (population < 0)
            {
                throw new ContinentException("Bevolkingsaantal kan niet kleiner zijn dan 0.");
            }
            Population = population;
        }
        //extra
        //public int GetPopulation()
        //{
        //    return Population;
        //}

        //public void RemoveCountry(Country country)
        //{
        //    if (_countries.Contains(country))
        //        _countries.Remove(country);
        //    else throw new ContinentException("Continent: the givzn country is not part of the continent that was given!");
        //}
    }
}
