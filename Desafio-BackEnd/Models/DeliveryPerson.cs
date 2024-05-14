using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Models
{
    [Table("DeliveryPeople")]
    [Comment("AKA Users, Couriers, etc")]
    [Index(nameof(Cnpj), nameof(CnhNumber), IsUnique = true)]
    public class DeliveryPerson
    {
        [Key]
        public int Id { get; set; }

        public string? IdentityUserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Cnpj { get; set; }

        public string CnhNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string CnhType { get; set; }

        public string CnhPicture { get; set; }

        public ICollection<Rental> Rentals { get; } = [];

        public IdentityUser? User { get; set; }
    }
}