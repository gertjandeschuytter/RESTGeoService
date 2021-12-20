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
            {
                throw new CountryException("Continent kan niet null zijn.");
            }
            Continent = continent;
        }
    }
}
