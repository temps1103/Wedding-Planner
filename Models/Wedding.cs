using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        [Required]
        [MinLength(2)]
        public string Wedder_One {get;set;}

        [Required]
        [MinLength(2)]
        public string Wedder_Two {get;set;}

        [Required]
        [DataType(DataType.Date)]
        [FutureDateCheck]
        public DateTime Date {get;set;}

        [Required]
        public string Wedding_Address {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;




        // User Creating the Wedding (ex Id #1)
        public int UserId {get;set;}

        // The User Creatiing the Wedding (bookmark to User Info)
        public User Wedding_Planner {get;set;}
        
        // Connect to Many to Many
        public List<Guest> Users_Already_RSVP {get;set;}
    }

    public class FutureDateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime minDate = (DateTime)value;
            return minDate <= DateTime.Now ? new ValidationResult("Can't go back in time! Please schedule for a Future Date.") : ValidationResult.Success;
        }
    }
}