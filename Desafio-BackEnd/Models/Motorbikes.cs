using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Models
{
    [Table("Motorbikes")]
    [Index(nameof(Plate), IsUnique = true)]
    public class Motorbike
    {
        [Required]
        public required string AdminId { get; set; }

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Modelo é obrigatório.")]
        [Display(Name = "Modelo")]
        [StringLength(100, ErrorMessage = "{0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 3)]
        public required string Model { get; set; } 

        [Required(AllowEmptyStrings = false, ErrorMessage = "Placa é obrigatório.")]
        [Display(Name = "Placa")]
        [RegularExpression("[A-Z]{3}[0-9][0-9A-Z][0-9]{2}", ErrorMessage = "A Placa não adere ao padrão Brasileiro (Ex: ABC0123)")]
        [StringLength(7, ErrorMessage = "{0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 7)]
        public required string Plate { get; set; }

        [Required]
        [Display(Name = "Ano de Fabricação")]
        [Range(1800, 9999, ErrorMessage = "Data inválida")]
        public int ProductionYear { get; set; }

        public ICollection<Rental> Rentals { get; } = [];
    }
}