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
            Name = name;
            Length = length;
        }

        private List<Country> CountriesWhereRiver { get; set; } = new List<Country>();

        public void SetCountries(List<Country> countries)
        {
            if (countries == null) throw new RiverException("River: List of countries is null");
            if (countries.Count < 1) throw new RiverException("River: River must belong to at least one country");
            else
            {
                //als er dubbels in zitten dan is het niet gelijk, zie distinct methode
                if (countries.Count == countries.Distinct().Count())
                {
                    //verwijder de rivier van elk land
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
                else throw new RiverException("River: The list of countries contains atleast one or more times the same value");
            }
        }
        public IReadOnlyList<Country> GetCountries()
        {
            return CountriesWhereRiver.AsReadOnly();
        }

        public int Id { get; set; }

        public void ZetId(int id)
        {
            if (id <= 0) throw new ContinentException("Id moet groter zijn dan 0.");
            Id = id;
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new RiverException("River: A ruver's name can not be null or empty");
                else _Name = value;
            }
        }

        private int _Length;
        public int Length
        {
            get { return _Length; }
            set
            {
                if (value < 1)
                    throw new RiverException("River: A river's length must be longer than 0");
                else _Length = value;
            }
        }
    }
}
