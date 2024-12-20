using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Models

{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters")]
        public string LastName { get; set; }

        public string? Gender { get; set; }

        public string? Race { get; set; }

        public DateOnly DoB { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage = "Invalid E-mail format")]
        [StringLength(50, ErrorMessage = "E-mail cannot exceed 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(50, ErrorMessage = "Phone Number cannot exceed 50 characters")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Postcode { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }
    }
}