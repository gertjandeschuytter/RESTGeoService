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
        }
        public Continent(string name)
        {
            ZetNaam(name);
        }
        public IReadOnlyList<Country> GetCountries()
        {
            return _countries.AsReadOnly();
        }
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
        public int GetPopulation()
        {
            int aantal = 0;
            foreach (Country l in _countries)
            {
                aantal += l.Population;
            }
            return aantal;
        }
        public void RemoveCountry(Country country)
        {
            if (_countries.Contains(country))
                _countries.Remove(country);
            else throw new ContinentException("Continent: the givzn country is not part of the continent that was given!");
        }
        public void ZetlandOpContinent(Country c)
        {
            if (!c.Continent.Equals(this))
                throw new ContinentException("Continent: the continent of this country did not equal this continent");
            foreach (Country r in _countries)
            {
                if (r.Equals(c))
                    throw new ContinentException("Continent: this country is allready part of this continent!");
                else if (r.Name == c.Name)
                    throw new ContinentException("Continent: The name of the country must be unique within the continent!");
            }
            _countries.Add(c);
        }
        public void ZetLanden(List<Country> countries)
        {
            List<Country> newCountries = new List<Country>();
            foreach (Country p in countries)
            {
                newCountries.Add(p);
            }
            _countries = newCountries;
        }
        public void VerwijderLandVanContinent(Country c)
        {
            if (_countries.Contains(c))
                _countries.Remove(c);
            else throw new ContinentException("Continent: the given country is not part of the continent that was given!");
        }

    }
}
