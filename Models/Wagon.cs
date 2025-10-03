namespace BlazorMetTailwind.Models{
    public class Wagon
    {
        public List<Dier> Dieren { get; set; } = new List<Dier>();
        public bool IsExperimental { get; set; }

        public int HuidigeCapaciteit => Dieren.Sum(dier => dier.SizeValue);

    }
}