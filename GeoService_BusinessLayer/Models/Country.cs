using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models {
    public class Country {

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Population { get; private set; }
        public decimal Surface { get; private set; }
        public Continent Continent { get; private set; }
        private List<River> Rivers { get; set; } = new();
        private List<City> cities { get; set; } = new();
        private List<City> capitals { get; set; } = new();
        //checken of dit werkt bij het finaliseren

        public Country(int id, string name, int population, decimal surface, Continent continent) : this(name, population, surface, continent)
        {
            ZetId(id);
        }

        public Country(string name, int population, decimal surface, Continent continent)
        {
            ZetNaam(name);
            ZetBevolkingsaantal(population);
            ZetOppervlakte(surface);
            ZetContinent(continent);
        }

        public void AddRiver(River river)
        {
            Rivers.Add(river);
        }
        public void RemoveRiver(River r)
        {
            Rivers.Remove(r);
        }
        //setters
        public void ZetId(int id)
        {
            if (id <= 0)
            {
                throw new CountryException("Id moet groter zijn dan 0.");
            }
            Id = id;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam))
            {
                throw new CountryException("Naam mag niet leeg zijn.");
            }
            Name = naam;
        }
        public void ZetBevolkingsaantal(int bevolkingsaantal)
        {
            if (bevolkingsaantal <= 0)
            {
                throw new CountryException("Bevolkingsaantal kan niet kleiner of gelijk zijn dan 0.");
            }
            Population = bevolkingsaantal;
        }
        public void ZetOppervlakte(decimal oppervlakte)
        {
            if (oppervlakte <= 0)
            {
                throw new CountryException("Oppervlakte kan niet kleiner of gelijk zijn dan 0.");
            }
            Surface = oppervlakte;
        }
        public void ZetContinent(Continent continent)
        {
            if (continent == null)
                throw new CountryException("Country: Country can't be null!");
            else
            {
                if (Continent != null)
                {
                    Continent.VerwijderLandVanContinent(this);
                }
                Continent = continent;
                Continent.ZetlandOpContinent(this);
            }
        }

        public IReadOnlyList<City> GetCities()
        {
            return cities.AsReadOnly();
        }
        public IReadOnlyList<City> GetCapitals()
        {
            return capitals.AsReadOnly();
        }
        public IReadOnlyList<River> GetRivers()
        {
            return Rivers.AsReadOnly();
        }
        public void ZetHoofdstad(City c)
        {
            if (!c.IsCapital)
                throw new CountryException("Country: The city was not a capital");
            else if (capitals.Contains(c))
                throw new CountryException("Country: This city is allready a capital of this country");
            else if (!cities.Contains(c))
            {
                throw new CountryException("Country: This city is not a part of this country");
            }
            capitals.Add(c);
        }
        public void ZetStad(City c)
        {
            if (cities.Contains(c))
                throw new CountryException("This city is allready oart of this country");
            else if (c.Country != this)
                throw new CountryException("Country: The country of this city did not equal this country");
            else
            {
                int total = 0;
                foreach (City r in cities)
                {
                    total += r.Population;
                }
                total += c.Population;
                if (total > Population)
                    throw new CountryException("Country: The population of ths cities in a country can not be bigger than the" +
                        " population of that country");
                cities.Add(c);
            }
        }
        public void RemoveCity(City c)
        {
            cities.Remove(c);
            capitals.Remove(c);
        }
        public void RemoveAsCapital(City c)
        {
            if (capitals.Contains(c))
            {
                capitals.Remove(c);
                c.ZetIsHoofdstad(false);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Country country &&
                   Name == country.Name &&
                   Population == country.Population &&
                   Surface == country.Surface;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Population, Surface);
        }
    }
}

