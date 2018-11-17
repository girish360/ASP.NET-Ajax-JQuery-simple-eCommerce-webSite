using DAL;
using DAL.Manager;
using Market.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{

    public class HomeController : Controller
    {
        UserVM actUser = new UserVM();

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Page = "Index";
            StoreContext st = new StoreContext();
            InitMarket();
            return View(actUser);
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            InitMarket();
            UserManager usMan = new UserManager();
            User tmp;
            if (usMan.Login(username, password, out tmp))
            {
                Session["user"] = tmp;

                HttpCookie cookie = new HttpCookie("user");
                cookie.Values["userName"] = tmp.UserName;
                cookie.Values["firstName"] = tmp.FirstName;
                cookie.Values["lastName"] = tmp.LastName;
                cookie.Values["ID"] = tmp.ID.ToString();
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookie);

            }
            else
            {
                ViewBag.loginError = "Wrong data";
            }
            ViewBag.Page = "Index";
            return View(actUser);
        }

        public ActionResult LogOut()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            ViewBag.Page = "Index";
            return RedirectToAction("Index", actUser);
        }

        public ActionResult LogIn()
        {
            InitMarket();

            if (Request.Cookies["user"] == null)
            {
                return View(actUser);
            }
            return View("Welcom", actUser);
        }

        public ActionResult Menu()
        {
            return View(actUser);
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Page = "Register";
            if (Request.Cookies["user"] == null)
                return View();

            UserVM user = new UserVM();
            User myUser = new DAL.User();
            if (Session["user"] == null)
            {
                UserManager tmpUM = new UserManager();
                Int64 id;
                if (Int64.TryParse(Request.Cookies["user"]["ID"].ToString(), out id))
                    myUser = tmpUM.GetUser(id);

            }
            else
            {
                myUser = (User)Session["user"];
            }

            if (myUser == null) return View();

            user.Birthdate = myUser.Birthdate;
            user.Email = myUser.Email;
            user.FirstName = myUser.FirstName;
            user.LastName = myUser.LastName;
            user.Password = myUser.Password;
            user.UserName = myUser.UserName;

            return View(user);
        }

        [HttpPost]
        public ActionResult Register(UserVM user)
        {
            UserManager userMan = new UserManager();
            bool flag = false;
            if (userMan.IsExist(user.UserName))
            {
                ModelState.AddModelError("UserName", "This username already exist!!");
                flag = true;
            }
            if (user.Password != user.PasswordValidation)
            {
                ModelState.AddModelError("password", "The password are not matching!");
                flag = true;
            }
            ViewBag.Page = "Register";
            if (flag) return View("Register");

            User tmp = new DAL.User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password
            };

            if (Request.Cookies["user"] == null)
            {
                if (userMan.Add(tmp))
                {

                    Session["user"] = tmp;

                    HttpCookie cookie = new HttpCookie("user");
                    cookie.Values["userName"] = tmp.UserName;
                    cookie.Values["firstName"] = tmp.FirstName;
                    cookie.Values["lastName"] = tmp.LastName;
                    cookie.Values["ID"] = tmp.ID.ToString();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);

                    ViewBag.Page = "Index";
                    return View("Index");
                }
                ViewBag.Message = "Wrong Data";
                ViewBag.Page = "Register";
                return View("Register");
            }
            else
            {
                var test = Request.Cookies["user"]["ID"];
                if (Int64.TryParse(Request.Cookies["user"]["ID"].ToString(), out Int64 ID))
                {
                    if (userMan.Update(ID, tmp))
                    {
                        Session["user"] = tmp;

                        Response.Cookies["user"]["userName"] = tmp.UserName;
                        Response.Cookies["user"]["firstName"] = tmp.FirstName;
                        Response.Cookies["user"]["lastName"] = tmp.LastName;
                        Response.Cookies["user"].Expires = DateTime.Now.AddDays(30);

                        ViewBag.Page = "Index";
                        return View("Index");
                    }
                }
                ViewBag.Message = "Wrong Data";
                ViewBag.Page = "Register";
                return View("Register");
            }
        }

        [HttpGet]
        public ActionResult InsertProduct()
        {
            ViewBag.Page = "InsertProduct";
            return View();

        }

        [HttpPost]
        public ActionResult InsertProduct(ProductVM prod, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            List<Byte[]> byteList = new List<byte[]>();
            Byte[] Pict1;
            if (IsImage(image1)) { Pict1 = toByteArr(image1); byteList.Add(Pict1); }

            Byte[] Pict2;
            if (IsImage(image2)) { Pict2 = toByteArr(image2); byteList.Add(Pict2); }

            Byte[] Pict3;
            if (IsImage(image3)) { Pict3 = toByteArr(image3); byteList.Add(Pict3); }

            ProductManager pm = new ProductManager();
            UserManager um = new UserManager();
            Int64 id;
            if (Int64.TryParse(Request.Cookies["user"]["ID"].ToString(), out id))
            {
                if (pm.Add(um.GetUser(id), prod.Title, prod.ShortDescription, prod.LongDescription, prod.Price, byteList))
                {
                    ViewBag.Page = "";
                    return View("CongratulationsView", prod);
                }
            }

            ViewBag.Page = "InsertProduct";
            return View("InsertProduct", prod);

        }

        public ActionResult AboutUs()
        {
            ViewBag.AboutUs = new string[] {"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam volutpat ultrices eros, et fermentum mi. Proin turpis neque, semper in elit id, rhoncus auctor nunc. Suspendisse vestibulum quam nec ipsum eleifend sodales. Etiam facilisis tincidunt eleifend. Curabitur condimentum lacus non ligula tincidunt congue. Curabitur ornare, enim in placerat condimentum, orci massa dapibus turpis, vulputate tincidunt ex dui a est. Sed faucibus accumsan ante, nec ornare velit rhoncus vel. Cras nec libero ac libero lobortis consequat nec sit amet lacus. Vivamus dapibus cursus vehicula. Sed viverra vel odio in tempus. Nunc metus libero, dictum accumsan felis vitae, ultrices accumsan nisl. ",
"Aenean varius auctor lacus vel egestas. Morbi arcu quam, cursus non semper eget, finibus vel dui. Sed mattis porta neque et bibendum. Suspendisse maximus porttitor convallis. Sed fermentum egestas sem, a ornare metus facilisis nec. Nam gravida ex et dolor eleifend, sit amet semper ipsum ornare. Aenean eu rutrum magna, ut luctus purus. Donec at tortor et enim ornare lacinia finibus id metus. Quisque egestas finibus libero et maximus. Duis porta lectus ligula, in consectetur lorem pretium quis. Suspendisse at ornare augue, a finibus ligula. Vivamus massa dui, laoreet non elit at, euismod rhoncus diam. Nam faucibus, lorem vel elementum ornare, ipsum est congue enim, ut vulputate ligula urna in orci.Quisque euismod nisl quis tortor pharetra vestibulum.",
"In sapien tellus, posuere in aliquet vitae, placerat gravida odio. Nullam porta turpis vitae blandit finibus. Nunc eu massa maximus lectus accumsan euismod.Duis iaculis tortor turpis, imperdiet tempus enim finibus vel. Etiam in quam nunc. Etiam at egestas dui. Curabitur consectetur leo eu magna vehicula, et vehicula neque malesuada.Etiam accumsan diam urna, in varius leo ultricies ac. ",
"Phasellus nunc tortor, rutrum in commodo vitae, porta eu tellus. Suspendisse feugiat aliquet imperdiet. Nullam rutrum dapibus sem, et hendrerit mauris hendrerit ut. Nullam ac urna commodo, facilisis magna a, suscipit felis.Morbi cursus ligula vitae neque dapibus, eget feugiat dui semper.In vitae orci pretium, dictum nulla id, laoreet risus.Donec pharetra fringilla lacus, et posuere odio congue at. Proin et sapien et ante vestibulum sollicitudin sed vel lectus. ",
"Vivamus lobortis, nisi in consectetur euismod, sapien risus fermentum sem, quis finibus est nisl sit amet ex.Morbi dui nisl, consectetur nec interdum pretium, molestie ut nibh. Morbi metus mauris, sagittis non nisl vitae, mattis congue tellus. Donec tempor orci volutpat purus dignissim faucibus.Etiam hendrerit arcu quis purus malesuada lobortis.Suspendisse ac fringilla orci, ut commodo ex. Fusce ac ultricies lorem. Pellentesque ullamcorper pretium mollis. Nam eget neque non lectus tincidunt volutpat vel condimentum sapien. Donec at commodo ex, at consequat est. Mauris mollis iaculis felis. " };
            ViewBag.Page = "AboutUs";
            return View();
        }

        public ActionResult Buy(string BoughtList)
        {
            ProductManager pm = new ProductManager();
            string[] arr = BoughtList.Split(',');
            List<Product> prodList = (List<Product>)Session["cart"];

            if (prodList != null)
            {
                foreach (Product item in prodList)
                {
                    if (arr.Contains(item.ID.ToString()))
                    {
                        bool flag = true;
                        if (Request.Cookies["user"] != null)
                            if (Request.Cookies["user"]["ID"] != null)
                                if (Int64.TryParse(Request.Cookies["user"]["ID"], out Int64 userID))
                                {
                                    flag = false;
                                    pm.BuyProduct(item.ID, userID);
                                }
                        if (flag)
                        {
                            pm.BuyProduct(item.ID);
                        }
                    }
                    else
                    {
                        pm.RemoveFromCart(item.ID);
                    }
                }
            }
            Session["cart"] = null;
            ViewBag.Page = "Index";
            return View("Index");
        }

        public ActionResult Cart()
        {
            if (Session["cart"] != null)
            {
                List<Product> cartPicks = (List<Product>)Session["cart"];
                actUser.Cart = new List<Product>();
                for (int i = 0; i < cartPicks.Count; i++)
                {
                    actUser.Cart.Add(cartPicks[i]);
                }
            }
            ViewBag.Page = "Cart";
            return View("Cart", actUser);
        }

        public ActionResult AddToCart(Int64? id)
        {
            ProductManager prodMan = new ProductManager();

            if (id == null)
            {
                return View("Index", actUser);
            }
            else
            {

                Int64 prodID = id ?? default(Int64);
                Product prod = prodMan.GetProduct(prodID);

                if (prod != null)
                {
                    if (!prodMan.AddToCart(prod.ID))
                    {
                        return View("Error");
                    }

                    List<Product> listID;

                    if (Session["cart"] == null) listID = new List<Product>();
                    else listID = (List<Product>)Session["cart"];

                    listID.Add(prod);
                    Session["cart"] = listID;
                }

                InitMarket();
                return View("Index", actUser);
            }
        }

        public ActionResult Market()
        {
            InitMarket();

            return View();
        }

        public ActionResult ProductDetail(Int64? id)
        {
            ViewBag.Page = "";

            if (id == null)
            {
                return View("Error");
            }
            else
            {
                ProductManager pm = new ProductManager();
                Int64 ID = id ?? default(Int64);
                Product prod = pm.GetProduct(ID);

                if (prod == null) return View("Error");

                ProductVM prodVM = new ProductVM()
                {
                    Title = prod.Title,
                    ShortDescription = prod.ShortDescription,
                    LongDescription = prod.LongDescription,
                    Price = prod.Price,
                    ID = prod.ID,
                    Date = prod.Date

                };

                if (prod.Picture1 != null)
                    prodVM.Picture1 = (prod.Picture1);
                if (prod.Picture2 != null)
                    prodVM.Picture2 = (prod.Picture2);
                if (prod.Picture3 != null)
                    prodVM.Picture3 = (prod.Picture3);

                return View(prodVM);
            }


        }

        public ActionResult _MarketByName()
        {
            ProductManager pm = new ProductManager();
            return PartialView("_SortedMarket",pm.GetAllAvailableProductByTitle());
        }

        public ActionResult _MarketByDate()
        {
            ProductManager pm = new ProductManager();
            return PartialView("_SortedMarket", pm.GetAllAvailableProductByDate());
        }

        public ActionResult _MarketByPrice()
        {
            ProductManager pm = new ProductManager();
            return PartialView("_SortedMarket", pm.GetAllAvailableProductByPrice());
        }




        public JsonResult VerifUserName(string UserName)
        {
            UserManager userM = new UserManager();
            bool result = !userM.IsExist(UserName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public void InitMarket()
        {
            ProductManager pm = new ProductManager();
            List<Product> productList = pm.GetAllAvailableProductByDate().ToList();
            ViewBag.Products = productList;
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            if (file.ContentType.Contains("image")) return true;


            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }


        public byte[] toByteArr(HttpPostedFileBase pict)
        {
            byte[] data;
            using (Stream inputStream = pict.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
                return data;
            }
        }

        public HttpPostedFileBase ByteToHttpPostedFileBase(byte[] data)
        {
            var contentType = "application/x-rar-compressed";
            var fileName = "images.rar";
            ViewClasses.HttpPostedFileBaseCustom HttpPostedFileBaseCustom = new ViewClasses.HttpPostedFileBaseCustom(GetSteamFromByeArray(data), contentType, fileName);
            return (HttpPostedFileBaseCustom);

        }
        public MemoryStream GetSteamFromByeArray(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);

            return ms;
        }



    }
}