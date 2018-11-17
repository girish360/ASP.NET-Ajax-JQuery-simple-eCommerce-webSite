using ClassLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manager
{
    public class UserManager : IUser
    {
        public bool Add(string firstName, string lastName, DateTime birthdate, string email, string userName, string password)
        {

            using (StoreContext stContext = new StoreContext())
            {
                var isExist = stContext.UserList.FirstOrDefault(u => u.UserName == userName);
                if (isExist == null && UserValidation.Validation(firstName, lastName, email, password, birthdate))
                {
                    stContext.UserList.Add(new User()
                    {
                        Birthdate = birthdate,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserName = userName,
                        Password = password
                    });
                    stContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Add(User user)
        {
            return Add(user.FirstName, user.LastName, user.Birthdate, user.Email, user.UserName, user.Password);
        }

        public User GetUser(Int64 id)
        {
            using (StoreContext stContext = new StoreContext())
            {
                return stContext.UserList.FirstOrDefault(u => u.ID == id);

            }

        }

        public bool IsExist(string userName)
        {
            using(var context = new StoreContext())
            {
                var tmp = (from u in context.UserList select u).FirstOrDefault(a => a.UserName == userName);
                return (tmp != null);
            }
        }

        public bool Login(string userName, string password, out User targetUser)
        {
            targetUser = null;
            if (userName == string.Empty || userName == null || password == string.Empty || password == null) return false;
            using (StoreContext stContext = new StoreContext())
            {

                var user = (from u in stContext.UserList
                            where userName == u.UserName && password == u.Password
                            select u).FirstOrDefault() ;
                if(user != null)
                {
                    targetUser = user;
                    return true;
                }
                return false;
            }
        }

        public bool Update(Int64 id, User user)
        {
            using (StoreContext context = new StoreContext())
            {
                var tmpUser = (from u in context.UserList select u).FirstOrDefault(u => u.ID == id);
                if (tmpUser != null)
                {
                    tmpUser.LastName = user.LastName;
                    tmpUser.Birthdate = user.Birthdate;
                    tmpUser.Email = user.Email;
                    tmpUser.FirstName = user.FirstName;
                    tmpUser.Password = user.Password;
                    tmpUser.UserName = user.UserName;
                    //context.Entry(tmpUser).CurrentValues.SetValues(user) ;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }


    }
}
