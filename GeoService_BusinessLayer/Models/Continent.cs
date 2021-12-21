using GeoService_BusinessLayer.ModelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoService_BusinessLayer.Models
{
    public class Continent
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Population { get; private set; }

        public Continent(int id, string name, int population) : this(name,population)
        {
            ZetId(id);
            ZetNaam(name);
            ZetBevolkingsaantal(population);
        }
        public Continent(string name, int population) : this(name)
        {
            ZetNaam(name);
            ZetBevolkingsaantal(population);
        }
        public Continent(string name)
        {
            ZetNaam(name);
        }

        //Setters
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
    }
}
