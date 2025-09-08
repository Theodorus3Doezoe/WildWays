using System.ComponentModel.DataAnnotations;

namespace BlazorMetTailwind.Data
{
    public class Dier
    {
        [Required(ErrorMessage = "Naam is een verplicht veld.")]
        public string? Naam { get; set; }

        [Required(ErrorMessage = "Grootte is een verplicht veld.")]
        public string? Grootte { get; set; }

        public string? Geslacht { get; set; }

        [Required(ErrorMessage = "Dieet is een verplicht veld.")]
        public string? Dieet { get; set; }

        public DateTime? Geboortedatum { get; set; }

        [Required(ErrorMessage = "Locatie is een verplicht veld.")]
        public string? Locatie { get; set; }

        public DateTime AanmaakDatum { get; set; }

        public bool Deleted { get; set; } = false;
    }
        
}