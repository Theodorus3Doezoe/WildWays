// In BlazorMetTailwind/Data/Wagon.cs
namespace BlazorMetTailwind.Data
{
    public class Wagon
    {
        public List<Dier> Dieren { get; set; } = new List<Dier>();
        public bool IsExperimental { get; set; }
        public int HuidigeCapaciteit => Dieren.Sum(dier => GetSizeValue(dier));

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
    }
}