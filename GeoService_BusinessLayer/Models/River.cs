using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models {
    public class River {
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
            if (countries == null) throw new RiverException("River: List of countries is null");
            if (countries.Count < 1) throw new RiverException("River: River must belong to at least one country");
            if (countries.Count != countries.Distinct().Count()) throw new RiverException("River: The list of countries contains atleast one or more times the same value");
            foreach (Country c in countries)
            {
                CountriesWhereRiver.Add(c);
                c.AddRiver(this);
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
