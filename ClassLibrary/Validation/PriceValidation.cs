using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Validation
{
    class PriceValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var price = (double)value;
            return (price != null && price>0);

        }
    }
}
