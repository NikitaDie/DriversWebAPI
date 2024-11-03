using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriversManagement.API.Models
{
    [Table("Drivers")]
    public class Driver
    {
        [Key] // Primary key
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Max length is 20")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [NotMapped] // Not added to the database
        public string FullName => $"{FirstName} {LastName}";

        // Foreign key property
        public int CategoryId { get; set; }
        
        // Navigation property
        public VehicleCategory Category { get; set; }
        
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public int Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string? LicenceNumber { get; set; }
    }
}