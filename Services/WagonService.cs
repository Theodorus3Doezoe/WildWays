using BlazorMetTailwind.Models; 

namespace BlazorMetTailwind.Services 
{
    public class WagonService
    {
        public List<Wagon> DeelDierenIn(List<Dier> dierenLijst)
        {
            const int maxNormalWagonCapacity = 10;
            const int maxExperimentalWagons = 4;

            var wagons = new List<Wagon>();
            var experimentalWagonsCount = 0;

            var gesorteerdeDieren = dierenLijst
                .Where(d => !d.Deleted)
                .OrderByDescending(d => d.Dieet == "Carnivoor")
                .ThenByDescending(d => d.SizeValue) 
                .ToList();

            if (!gesorteerdeDieren.Any())
            {
                return wagons;
            }

            foreach (var dier in gesorteerdeDieren)
            {
                bool geplaatst = false;

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

                foreach (var normalWagon in wagons.Where(w => !w.IsExperimental))
                {
                    if (CanAddAnimalToNormalWagon(dier, normalWagon.Dieren) && (normalWagon.HuidigeCapaciteit + dier.SizeValue) <= maxNormalWagonCapacity)
                    {
                        normalWagon.Dieren.Add(dier);
                        geplaatst = true;
                        break;
                    }
                }
                if (geplaatst) continue;

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

                var nieuweNormaleWagon = new Wagon { IsExperimental = false };
                if ((nieuweNormaleWagon.HuidigeCapaciteit + dier.SizeValue) <= maxNormalWagonCapacity)
                {
                    nieuweNormaleWagon.Dieren.Add(dier);
                    wagons.Add(nieuweNormaleWagon);
                }
            }
            return wagons;
        }

        private bool CanAddAnimalToNormalWagon(Dier nieuwDier, List<Dier> dierenInWagon)
        {
            if (nieuwDier.Dieet == "Carnivoor")
            {
                foreach (var aanwezigDier in dierenInWagon)
                {
                    if (aanwezigDier.SizeValue >= nieuwDier.SizeValue)
                    {
                        return false;
                    }
                }
            }
            else // Herbivoor
            {
                foreach (var aanwezigDier in dierenInWagon)
                {
                    if (aanwezigDier.Dieet == "Carnivoor" && aanwezigDier.SizeValue >= nieuwDier.SizeValue)
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
    }
}