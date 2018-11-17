using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Validation
{
    public static class UserValidation //: ValidationAttribute
    {
        public static bool Validation(string firstName, string lastName, string email, string password, DateTime birthDate)
        {
            bool flag = true; ;
            EmailValidation ev = new EmailValidation();
            BirthdateValidator bv = new BirthdateValidator();


            flag = flag && firstName.Length > 0 && firstName.Length <= 50
                && lastName.Length > 0 && lastName.Length <= 50
                && email.Length > 0 && email.Length <= 50 && ev.IsValid(email)
                && password.Length > 0 && password.Length <= 50
                && bv.IsValid(birthDate);

            return flag;
        }
    }
}
