using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Backend.Models
{

    public class MotorbikeLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public int MotorbikeId { get; set; }

        public Motorbike Motorbike { get; set; } = null!;
    }
}