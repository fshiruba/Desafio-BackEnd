using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_Backend.Models
{
    [Table("Plans")]
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        public int RentalDays { get; set; }

        public int RentalCostPerDay { get; set; }
        public int PenaltyFeePercent { get; set; }

        public ICollection<Rental> Rentals { get; } = [];
    }
}