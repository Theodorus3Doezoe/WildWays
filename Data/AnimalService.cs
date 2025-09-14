// In BlazorMetTailwind/Data/AnimalService.cs
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

        private int GetSizeValue(Dier dier)
        {
            return dier.Grootte switch
            {
                "Klein" => 1,
                "Middel" => 3,
                "Groot" => 5,
                _ => 0,
            };
        }
        
        private bool CanAddAnimalToNormalWagon(Dier nieuwDier, List<Dier> dierenInWagon)
        {
            if (nieuwDier.Dieet == "Carnivoor")
            {
                foreach (var aanwezigDier in dierenInWagon)
                {
                    if (GetSizeValue(aanwezigDier) >= GetSizeValue(nieuwDier))
                    {
                        return false;
                    }
                }
            }
            else // Herbivoor
            {
                foreach (var aanwezigDier in dierenInWagon)
                {
                    if (aanwezigDier.Dieet == "Carnivoor" && GetSizeValue(aanwezigDier) >= GetSizeValue(nieuwDier))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CanAddAnimalToExpWagon(Dier nieuwDier, Wagon wagon)
        {
            if (nieuwDier.Grootte is "Groot") return false;
            if (wagon.Dieren.Count >= 2) return false;
            if (!wagon.Dieren.Any()) return true;

            var bestaandDier = wagon.Dieren.First();
            
            if (bestaandDier.Dieet == "Carnivoor" && nieuwDier.Dieet == "Carnivoor") return true;

            if ((bestaandDier.Dieet == "Carnivoor" && nieuwDier.Dieet == "Herbivoor") ||
                (bestaandDier.Dieet == "Herbivoor" && nieuwDier.Dieet == "Carnivoor")) return true;

            return false;
        }

        public List<Wagon> ArrangeAnimalsInWagons()
        {
            const int maxNormalWagonCapacity = 10;
            const int maxExperimentalWagons = 4;
            
            var wagons = new List<Wagon>(); 
            var experimentalWagonsCount = 0;

            var gesorteerdeDieren = _dierenLijst
                .Where(d => !d.Deleted)
                .OrderByDescending(d => d.Dieet == "Carnivoor")
                .ThenByDescending(d => GetSizeValue(d))
                .ToList();

            if (!gesorteerdeDieren.Any())
            {
                return wagons;
            }

            foreach (var dier in gesorteerdeDieren)
            {
                bool geplaatst = false;
                int dierGrootte = GetSizeValue(dier);

                // 1. Check bestaande experimentele wagons
                foreach (var expWagon in wagons.Where(w => w.IsExperimental))
                {
                    if (CanAddAnimalToExpWagon(dier, expWagon))
                    {
                        expWagon.Dieren.Add(dier);
                        geplaatst = true;
                        break;
                    }
                }
                if (geplaatst) continue;

                // 2. Check bestaande normale wagons
                foreach (var normalWagon in wagons.Where(w => !w.IsExperimental))
                {
                    if (CanAddAnimalToNormalWagon(dier, normalWagon.Dieren) && (normalWagon.HuidigeCapaciteit + dierGrootte) <= maxNormalWagonCapacity)
                    {
                        normalWagon.Dieren.Add(dier);
                        geplaatst = true;
                        break;
                    }
                }
                if (geplaatst) continue;
                
                // 3. Maak nieuwe experimentele wagon aan (indien mogelijk)
                var isEligibleForExpWagon = dier.Grootte is "Klein" or "Middel";
                if (isEligibleForExpWagon && experimentalWagonsCount < maxExperimentalWagons)
                {
                    var nieuweExpWagon = new Wagon { IsExperimental = true };
                    nieuweExpWagon.Dieren.Add(dier);
                    wagons.Add(nieuweExpWagon);
                    experimentalWagonsCount++;
                    geplaatst = true;
                }
                if (geplaatst) continue;

                // 4. Maak nieuwe normale wagon aan
                var nieuweNormaleWagon = new Wagon { IsExperimental = false };
                if ((nieuweNormaleWagon.HuidigeCapaciteit + dierGrootte) <= maxNormalWagonCapacity)
                {
                    nieuweNormaleWagon.Dieren.Add(dier);
                    wagons.Add(nieuweNormaleWagon);
                }
            }
            return wagons;
        }
    }
}