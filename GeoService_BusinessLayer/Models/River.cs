using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models {
    public class River {
        public River(string name, int length, List<Country> countries) : this(name, length)
        {
            SetCountries(countries);
        }
        public River(string name, int length)
        {
            ZetNaam(name);
            ZetLengte(length);
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Length { get; private set; }
        private List<Country> CountriesWhereRiver { get; set; } = new List<Country>();

        public void SetCountries(List<Country> countries)
        {
            if (countries == null || countries.Count < 1)
                throw new RiverException("River: River must belong to at least one country");
            else
            {
                if (countries.Count == countries.Distinct().Count())
                {
                    foreach (Country cr in CountriesWhereRiver)
                    {
                        cr.RemoveRiver(this);
                    }
                    CountriesWhereRiver = new List<Country>();
                    foreach (Country r in countries)
                    {
                        CountriesWhereRiver.Add(r);
                        r.AddRiver(this);
                    }
                }
                else throw new RiverException("River: The list of countries contained doubles");
            }
        }
        public IReadOnlyList<Country> GetCountries()
        {
            return CountriesWhereRiver.AsReadOnly();
        }

        //setters
        public void ZetId(int id)
        {
            if (id <= 0) throw new RiverException("Id moet groter zijn dan 0.");
            Id = id;
        }
        public void ZetNaam(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new RiverException("Id moet groter zijn dan 0.");
            Name = name;
        }
        public void ZetLengte(int length)
        {
            if (length < 1) throw new RiverException("River: A river's length must be longer than 0");
            Length = length;
        }
    }
}
