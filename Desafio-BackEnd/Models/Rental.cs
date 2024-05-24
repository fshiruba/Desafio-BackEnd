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

        public DateTime StartDate
        {
            get
            {
                if (startDate.Kind != DateTimeKind.Utc)
                {
                    startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
                }

                return startDate.Date;
            }

            set
            {
                startDate = value.Date;

                if (startDate.Kind != DateTimeKind.Utc)
                {
                    startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                if (endDate.HasValue && endDate.Value.Kind != DateTimeKind.Utc)
                {
                    endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                }

                return endDate.HasValue ? endDate.Value.Date : null;
            }

            set
            {
                endDate = value.HasValue ? value.Value.Date : null;

                if (endDate.HasValue && endDate.Value.Kind != DateTimeKind.Utc)
                {
                    endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                }
            }
        }

        public DateTime ExpectedEndDate
        {
            get
            {
                if (expectedEndDate.Kind != DateTimeKind.Utc)
                {
                    expectedEndDate = DateTime.SpecifyKind(expectedEndDate, DateTimeKind.Utc);
                }

                return expectedEndDate.Date;
            }

            set
            {
                expectedEndDate = value.Date;

                if (expectedEndDate.Kind != DateTimeKind.Utc)
                {
                    expectedEndDate = DateTime.SpecifyKind(expectedEndDate, DateTimeKind.Utc);
                }
            }
        }

        public int MotorbikeId { get; set; }

        public int DeliveryPersonId { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; } = null!;

        public DeliveryPerson DeliveryPerson { get; set; } = null!;

        public Motorbike Motorbike { get; set; } = null!;
    }
}