using ClassLibrary.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class User 
    {
        [Required]
        [Key]
        public Int64 ID { get; set; }

        [Required(ErrorMessage = "Please enter a First Name")]
        [StringLength(50, ErrorMessage ="Please, Enter a Firstname inferior to 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Last Name")]
        [StringLength(50, ErrorMessage = "Please, Enter a Lastname inferior to 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a BirthDate")]
        [BirthdateValidator( ErrorMessage = "Please, Enter a valid Birthdate")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail")]
        [StringLength(50, ErrorMessage = "Please, Enter an e-mail inferior to 50 characters.")]
        [EmailAddress(ErrorMessage = "Please, Enter a valid e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(50,MinimumLength = 2 , ErrorMessage = "Please, Enter a username between 2 to 50 characters.")]
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please, Enter a password between 2 to 50 characters.")]
        public string Password { get; set; }

    }
}
