using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Models
{
    [Table("Motorbikes")]
    [Index(nameof(Plate), IsUnique = true)]
    public class Motorbike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ano de Fabricação")]
        public int ProductionYear { get; set; }

        [Required]
        [Display(Name = "Modelo")]
        [StringLength(100, ErrorMessage = "{0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 3)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Placa")]
        public string Plate { get; set; }

        [Required]
        public string AdminId { get; set; }

        public ICollection<Rental> Rentals { get; } = [];
    }
}