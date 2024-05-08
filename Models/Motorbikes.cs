using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_Backend.Models
{
    [Table("Motorbikes")]
    [Index(nameof(Plate), IsUnique = true)]
    public class Motorbike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Plate { get; set; }

        [Required]
        public string AdminId { get; set; }

        public ICollection<Rental> Rentals { get; } = [];
    }
}