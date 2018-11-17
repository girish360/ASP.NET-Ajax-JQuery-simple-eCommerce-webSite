using ClassLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class ProductManager : IProduct
    {


        public void Add(Product product)
        {
            using (StoreContext stContext = new StoreContext())
            {
                stContext.ProductList.Add(product);
                stContext.SaveChanges();

            }
        }

        public bool Add(User ownerID, string title, string shortDescription, string longDescription, double price)
        {
            if (ProductValidation.Validation(ownerID, title, shortDescription, longDescription, price))
            {
                using (StoreContext stContext = new StoreContext())
                {
                    stContext.ProductList.Add(new Product()
                    {
                        OwnerID = ownerID,
                        Title = title,
                        ShortDescription = shortDescription,
                        LongDescription = longDescription,
                        Price = price,
                        Date = DateTime.Now,
                        state = ClassLibrary.State.Available
                    });
                    stContext.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public bool Add(User ownerID, string title, string shortDescription, string longDescription, double price,  List<Byte[]> images)
        {

            if (ProductValidation.Validation(ownerID, title, shortDescription, longDescription, price))
            {
                using (StoreContext stContext = new StoreContext())
                {
                    var userInDB = (from u in stContext.UserList select u).FirstOrDefault(us => us.ID == ownerID.ID);
                    if (userInDB == null) return false;



                    Product prod = new Product()
                {
                    OwnerID = userInDB,
                    Title = title,
                    ShortDescription = shortDescription,
                    LongDescription = longDescription,
                    Price = price,
                    Date = DateTime.Now,
                    state = ClassLibrary.State.Available,
                    CartTime = DateTime.Now.AddYears(1)
                };
                if (images.Count > 0)
                {
                    prod.Picture1 = images[0];
                    if (images.Count > 1)
                    {
                        prod.Picture2 = images[1];
                        if (images.Count > 2) prod.Picture3 = images[2];
                    }
                }



                    stContext.ProductList.Add(prod);
                    stContext.SaveChanges();
                    //var tmp = (from a in stContext.ProductList select a).FirstOrDefault(a => a == prod);
                    //AddImageToProduct(tmp.ID, images);
                }

                return true;
            }
            return false;
        }

        public bool AddImageToProduct(Int64 productID, params Byte[][] images)
        {
            if (images.Length > 3) return false;
            using (StoreContext stContext = new StoreContext())
            {
                var prod = (from p in stContext.ProductList where productID == p.ID select p).FirstOrDefault();
                if (prod == null) return false;
                if (images.Length > 0)
                {
                    prod.Picture1 = images[0];
                    if (images.Length > 1)
                    {
                        prod.Picture2 = images[1];
                        if (images.Length > 2) prod.Picture3 = images[2];
                    }
                    stContext.SaveChanges();

                    return true;
                }
                return false;
            }

        }

        public bool AddImageToProduct(Int64 productID, List<Byte[]> images)
        {
            if (images.Count > 3) return false;
            using (StoreContext stContext = new StoreContext())
            {
                var prod = (from p in stContext.ProductList where productID == p.ID select p).FirstOrDefault();
                if (prod == null) return false;
                if (images.Count > 0)
                {
                    prod.Picture1 = images[0];
                    if (images.Count > 1)
                    {
                        prod.Picture2 = images[1];
                        if (images.Count > 2) prod.Picture3 = images[2];
                    }
                    stContext.SaveChanges();

                    return true;
                }
                return false;
            }

        }

        public bool AddToCart(Int64 ProductID)
        {
            using (StoreContext stContext = new StoreContext())
            {
                var prod = (from p in stContext.ProductList where p.ID == ProductID select p).FirstOrDefault();
                if ( prod != null)
                {

                    prod.CartTime = DateTime.Now;
                    prod.state = ClassLibrary.State.InCart;

                    stContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool BuyProduct(Int64 ProductID, Int64 UserID = -1)
        {
            using (StoreContext stContext = new StoreContext())
            {
                var user = (from u in stContext.UserList where u.ID == UserID select u).FirstOrDefault();
                var prod = (from p in stContext.ProductList where p.ID == ProductID && p.state != ClassLibrary.State.Sold select p).FirstOrDefault();
                if (prod != null)
                {


                    prod.Date = DateTime.Now;
                    prod.state = ClassLibrary.State.Sold;
                    prod.UserID = user;

                    stContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveFromCart(Int64 ProductID)
        {
            using (StoreContext stContext = new StoreContext())
            {
                var prod = (from p in stContext.ProductList where p.ID == ProductID && p.state!=ClassLibrary.State.Sold select p).FirstOrDefault();
                if (prod != null)
                {


                    prod.state = ClassLibrary.State.Available;
                    prod.UserID = null;

                    stContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public ICollection<Product> GetAllAvailableProductByTitle()
        { 
            using (StoreContext stContext = new StoreContext())
            {
                var prod = from p in stContext.ProductList where p.state == ClassLibrary.State.InCart select p;
                foreach (var item in prod)
                {
                    if(item.CartTime == null)
                    {
                        item.state = ClassLibrary.State.Available;

                    }
                    else
                    {
                        TimeSpan tS = new TimeSpan();
                        tS = (DateTime.Now - item.CartTime);
                        if (tS.Minutes > 30)
                        {
                            item.state = ClassLibrary.State.Available;
                        }
                    }
                    
                }
                stContext.SaveChanges();

                var listProd = (from p in stContext.ProductList orderby p.Title ascending where p.state == ClassLibrary.State.Available select p).ToArray();
                return listProd;
            }
        }

        public ICollection<Product> GetAllAvailableProductByDate()
        {

            using (StoreContext stContext = new StoreContext())
            {
                var prod = from p in stContext.ProductList where p.state == ClassLibrary.State.InCart select p;
                foreach (var item in prod)
                {
                    if (item.CartTime == null)
                    {
                        item.state = ClassLibrary.State.Available;

                    }
                    else
                    {
                        TimeSpan tS = new TimeSpan();
                        tS = (DateTime.Now - item.CartTime);
                        if (tS.Minutes > 30)
                        {
                            item.state = ClassLibrary.State.Available;
                        }
                    }

                }
                stContext.SaveChanges();


                var listProd = (from p in stContext.ProductList orderby p.Date ascending where p.state == ClassLibrary.State.Available select p).ToArray();
                return listProd;
            }
        }

        public ICollection<Product> GetAllAvailableProductByPrice()
        {

            using (StoreContext stContext = new StoreContext())
            {
                var prod = from p in stContext.ProductList where p.state == ClassLibrary.State.InCart select p;
                foreach (var item in prod)
                {
                    if (item.CartTime == null)
                    {
                        item.state = ClassLibrary.State.Available;

                    }
                    else
                    {
                        TimeSpan tS = new TimeSpan();
                        tS = (DateTime.Now - item.CartTime);
                        if (tS.Minutes > 30)
                        {
                            item.state = ClassLibrary.State.Available;
                        }
                    }

                }
                stContext.SaveChanges();


                var listProd = (from p in stContext.ProductList orderby p.Price ascending where p.state == ClassLibrary.State.Available select p).ToArray();
                return listProd;
            }
        }

        public ICollection<Product> GetCartByUser(Int64 userId)
        {
            using (StoreContext stContext = new StoreContext())
            {
                var cart = (from p in stContext.ProductList
                            where p.UserID.ID == userId && p.state == ClassLibrary.State.InCart
                            select p).ToArray();
                return cart;
            }
        }

        public Product GetProduct(Int64 id)
        {
            using (StoreContext stContext = new StoreContext())
            {
                var Prod = stContext.ProductList.FirstOrDefault(p => p.ID == id);
                return Prod;
            }

        }

        
    }
}
