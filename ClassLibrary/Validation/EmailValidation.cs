using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Validation
{
    class EmailValidation:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                MailAddress m = new MailAddress(value as string);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
