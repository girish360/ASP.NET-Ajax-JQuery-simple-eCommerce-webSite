using ClassLibrary.Validation;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.Models
{
    public class ActualUser
    {

        public User user { get; set; }
        public List<Product> cart { get; set; }

        //[Required(ErrorMessage = "Please enter a username")]
        //[StringLength(50, ErrorMessage = "Please, Enter a username inferior to 50 characters.")]
        //[Display(Name = "User Name")]
        //public string UserName { get; set; }

        //[Required(ErrorMessage = "Please enter a Password")]
        //[StringLength(50, ErrorMessage = "Please, Enter a password inferior to 50 characters.")]
        //public string Password { get; set; }
    }
}