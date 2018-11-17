using ClassLibrary.Validation;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Models
{
    public class UserVM
    {
        //[Required]
        //public Int64 ID { get; set; }

        [Required(ErrorMessage = "Please enter a First Name")]
        [StringLength(50, ErrorMessage = "Please, Enter a Firstname inferior to 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Last Name")]
        [StringLength(50, ErrorMessage = "Please, Enter a Lastname inferior to 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a BirthDate")]
        [BirthdateValidator(ErrorMessage = "Please, Enter a valid Birthdate")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail")]
        [StringLength(50, ErrorMessage = "Please, Enter an e-mail inferior to 50 characters.")]
        [EmailAddress(ErrorMessage = "Please, Enter a valid e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please, Enter a username between 2 to 50 characters.")]
        [Display(Name = "User Name")]
        [Remote("VerifUserName", "Home", ErrorMessage = "This User Name already exist")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please, Enter a password between 2 to 50 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please, Enter a password between 2 to 50 characters.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string PasswordValidation { get; set; }


        public List<Product> Cart { get; set; }

    }
}