using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Models
{
    public class User
    {
        [Key]
        public int UserId   {get;set;}

        [Required]
        [MinLength(2)]
        public string First_Name {get;set;}

        [Required]
        [MinLength(2)]
        public string Last_Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;



        // Weddings User has Created 
        public List<Wedding> My_Weddings {get;set;}
        
        // Connect to Many to Many
        public List<Guest> Weddings_Attending {get;set;}


        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PW_Confirm {get;set;}
    }
}