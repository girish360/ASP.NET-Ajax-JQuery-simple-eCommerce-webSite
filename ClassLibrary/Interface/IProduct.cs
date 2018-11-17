using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IProduct
    {

        void Add(Product product);
        bool Add(User ownerID, string title, string shortDescription, string longDescription, double price);
        bool Add(User ownerID, string title, string shortDescription, string longDescription, double price, List<Byte[]> images);
        bool AddImageToProduct(Int64 productID, params Byte[][] images);
        bool AddImageToProduct(Int64 productID, List<Byte[]> images);
        bool AddToCart(Int64 ProductID);
        bool BuyProduct(Int64 UserID, Int64 ProductID);
        bool RemoveFromCart(Int64 ProductID);
        ICollection<Product> GetAllAvailableProductByTitle();
        ICollection<Product> GetAllAvailableProductByDate();
        ICollection<Product> GetAllAvailableProductByPrice();
        ICollection<Product> GetCartByUser(Int64 userId);

        Product GetProduct(Int64 id);

    }
}
