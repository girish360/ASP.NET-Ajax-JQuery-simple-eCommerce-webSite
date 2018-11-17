using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Validation
{
    public static class ProductValidation //: ValidationAttribute
    {
        public static bool Validation(User ownerID, string title, string shortDescription, string longDescription, double price)
        {
            bool flag = true;
            using(var context = new StoreContext())
            {
                var user = from u in context.UserList where u.ID == ownerID.ID select u;
            
            flag = flag && (user != null);
            }

            flag = flag && title.Length > 0 && title.Length <= 50
                && shortDescription.Length > 0 && shortDescription.Length <= 500
                && longDescription.Length > 0 && longDescription.Length <= 4000
                && price > 0; 


            return flag;
        }
    }
}
