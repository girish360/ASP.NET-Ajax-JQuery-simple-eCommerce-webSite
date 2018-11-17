using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext()
        {

        }
        public DbSet<User> UserList { get; set; }
        public DbSet<Product> ProductList { get; set; }
    }
}
