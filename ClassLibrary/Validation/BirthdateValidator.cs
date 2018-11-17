using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Validation
{
    public class BirthdateValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            if (value != null)
            {
                var date = (DateTime)value;
                return (date < DateTime.Now && date > new DateTime(1900, 01, 01));

            }

            return false;
        }
    }
     
}
