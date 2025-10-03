using BlazorMetTailwind.Models; 

namespace BlazorMetTailwind.Services 
{
    public class AnimalService
    {
        private readonly List<Dier> _dierenLijst = new()
        {
            new Dier { Naam = "Leo de Leeuw", Locatie = "Beekse Bergen", Grootte = "Groot", Dieet = "Carnivoor", Geboortedatum = new DateTime(2020, 5, 10), Geslacht = "Mannelijk" },
            new Dier { Naam = "Ahmed de Adder", Locatie = "Artis", Grootte = "Middel", Dieet = "Carnivoor", Geboortedatum = new DateTime(2022, 1, 20), Geslacht = "Mannelijk" },
            new Dier { Naam = "Zoe de Zebra", Locatie = "Beekse Bergen", Grootte = "Middel", Dieet = "Herbivoor", Geboortedatum = new DateTime(2019, 8, 1), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Mimi de Muis", Locatie = "Artis", Grootte = "Klein", Dieet = "Herbivoor", Geboortedatum = new DateTime(2023, 3, 15), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Elly de Olifant", Locatie = "Beekse Bergen", Grootte = "Groot", Dieet = "Herbivoor", Geboortedatum = new DateTime(2015, 11, 2), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Carla de Carnivoor", Locatie = "Artis", Grootte = "Klein", Dieet = "Carnivoor", Geboortedatum = new DateTime(2023, 1, 1), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Hennie de Herbivoor", Locatie = "Beekse Bergen", Grootte = "Middel", Dieet = "Herbivoor", Geboortedatum = new DateTime(2022, 1, 1), Geslacht = "Mannelijk" }
        };

        public List<Dier> GetDieren() => _dierenLijst;

        public bool AddDier(Dier nieuwDier)
        {
            if (_dierenLijst.Any(d => d.Naam.Equals(nieuwDier.Naam, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"De naam '{nieuwDier.Naam}' is al in gebruik.");
                return false;
            }
            _dierenLijst.Add(nieuwDier);
            return true;
        }

        public void SoftDeleteDier(Dier dierOmTeVerwijderen)
        {
            var dierInLijst = _dierenLijst.FirstOrDefault(d => d.Naam == dierOmTeVerwijderen.Naam);
            if (dierInLijst != null)
            {
                dierInLijst.Deleted = true;
            }
        }
    }
}