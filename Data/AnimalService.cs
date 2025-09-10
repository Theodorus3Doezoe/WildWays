namespace BlazorMetTailwind.Data

{
    public class AnimalService
    {
        private readonly List<Dier> _dierenLijst = new()
        {
            // Voorbeeldgegevens
            new Dier { Naam = "Leo de Leeuw", Locatie = "Beekse Bergen", Grootte = "Groot", Dieet = "Carnivoor", Geboortedatum = new DateTime(2020, 5, 10), Geslacht = "Mannelijk" },
            new Dier { Naam = "Ahmed de Adder", Locatie = "Artis", Grootte = "Middel", Dieet = "Carnivoor", Geboortedatum = new DateTime(2022, 1, 20), Geslacht = "Mannelijk" },
            new Dier { Naam = "Zoe de Zebra", Locatie = "Beekse Bergen", Grootte = "Middel", Dieet = "Herbivoor", Geboortedatum = new DateTime(2019, 8, 1), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Mimi de Muis", Locatie = "Artis", Grootte = "Klein", Dieet = "Herbivoor", Geboortedatum = new DateTime(2023, 3, 15), Geslacht = "Vrouwelijk" },
            new Dier { Naam = "Elly de Olifant", Locatie = "Beekse Bergen", Grootte = "Groot", Dieet = "Herbivoor", Geboortedatum = new DateTime(2015, 11, 2), Geslacht = "Vrouwelijk" }
        };

        public List<Dier> GetDieren() => _dierenLijst;

        public bool AddDier(Dier nieuwDier)
        {
            // Controleer of de naam al bestaat (hoofdletterongevoelig)
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

        private int GetSizeValue(Dier dier)
        {
            if (dier.Grootte == "Klein")
            {
                return 1;
            }
            else if (dier.Grootte == "Middel")
            {
                return 3;
            }
            else if (dier.Grootte == "Groot")
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }

        private bool CanAddAnimalToWagon(Dier nieuwDier, List<Dier> wagon)
        {
            if (nieuwDier.Dieet == "Carnivoor")
            {
                foreach (var aanwezigDier in wagon)
                {
                    if (GetSizeValue(aanwezigDier) >= GetSizeValue(nieuwDier))
                    {
                        return false; 
                    }
                }
            }
            else // Herbivoor
            {
                foreach (var aanwezigDier in wagon)
                {
                    if (aanwezigDier.Dieet == "Carnivoor" && GetSizeValue(aanwezigDier) >= GetSizeValue(nieuwDier))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<List<Dier>> ArrangeAnimalsInWagons()
        {
            const int maxWagonCapacity = 10;
            var wagons = new List<List<Dier>>();
            
            var gesorteerdeDieren = _dierenLijst
                .Where(d => !d.Deleted) 
                .OrderByDescending(d => d.Dieet == "Carnivoor")
                .ThenByDescending(d => GetSizeValue(d))
                .ToList();

            if (!gesorteerdeDieren.Any())
            {
                return wagons; 
            }

            var huidigeWagon = new List<Dier>();
            int huidigeCapaciteit = 0;
            wagons.Add(huidigeWagon);

            foreach (var dier in gesorteerdeDieren)
            {
                int dierGrootte = GetSizeValue(dier);

                bool geplaatst = false;
                foreach (var wagon in wagons)
                {
                    int wagonCapaciteit = wagon.Sum(GetSizeValue);
                    if (CanAddAnimalToWagon(dier, wagon) && (wagonCapaciteit + dierGrootte) <= maxWagonCapacity)
                    {
                        wagon.Add(dier);
                        geplaatst = true;
                        break;
                    }
                }

                if (!geplaatst)
                {
                    var nieuweWagon = new List<Dier> { dier };
                    wagons.Add(nieuweWagon);
                }
            }
            
            return wagons;
        }
    }
}