using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_Backend.Models
{
    [Table("Rentals")]
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        private DateTime startDate;
        private DateTime? endDate;
        private DateTime expectedEndDate;

        public DateTime StartDate { get => startDate.Date; set => startDate = value.Date; }

        public DateTime? EndDate { get => endDate?.Date; set => endDate = value?.Date; }
        public DateTime ExpectedEndDate { get => expectedEndDate.Date; set => expectedEndDate = value.Date; }

        public int MotorbikeId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int PlanId { get; set; }

        public Plan Plan { get; set; } = null!;

        public DeliveryPerson DeliveryPerson { get; set; } = null!;

        public Motorbike Motorbike { get; set; } = null!;
    }
}