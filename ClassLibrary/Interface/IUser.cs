using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUser
    {
        bool IsExist(string userName);

        bool Add(string firstName, string lastName, DateTime birthdate, string email, string userName, string password);

        bool Add(User user);

        User GetUser(Int64 id);

        bool Login(string userName, string password, out User targetUser);

        bool Update(Int64 id, User user);

    }
}
