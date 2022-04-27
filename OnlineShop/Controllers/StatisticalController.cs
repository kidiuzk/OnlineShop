using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin,Manage")]
    public class StatisticalController : Controller
    {
        Entities db = new Entities();
        // GET: Statistical
        public ActionResult Index()
        {
            DateTime now = DateTime.Today;
            ViewBag.Totalrevenue = Revenuestatistics();
            ViewBag.Totalorder = OrderStatistics();
            ViewBag.Numberofusers = UserStatistics();
            ViewBag.Monthlyrevenue = Monthlyrevenuestatistics(now.Month, now.Year);

            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(FormCollection f)
        {
            string username = f["txtUserName"].ToString();   //lấy chuỗi trong txtUserName
            string password = f["txtPassword"].ToString();    //lấy chuỗi trong txtPassword

            User user = db.Users.SingleOrDefault(n => n.UserName == username && n.Password == password);      //compare with username and password in database
            if (user != null)
            {
                var lstRole = db.UserType_Role.Where(n => n.IDUsertype == user.IDUsertype);   //get the list of corresponding permissions UserType
                string Role = "";
                if (lstRole.Count() != 0)
                {
                    foreach (var item in lstRole)   //browse list of permissions
                    {
                        Role += item.Role.RoleID + ",";
                    }
                    Role = Role.Substring(0, Role.Length - 1); //Cut the ,
                    Decentralization(user.UserName.ToString(), Role);

                    Session["UserName"] = user;
                    return RedirectToAction("Index");   //script used to reload the page after successful login
                }
            }
            return Content("Incorrect account or password.");
        }

        public ActionResult LogoutAdmin()
        {
            Session["UserName"] = null; //thiết lập session là null

            FormsAuthentication.SignOut();  //xóa bộ nhớ cookie

            return RedirectToAction("Index", "Statistical");
        }

        public ActionResult Authorizationword()
        {
            return View();
        }

        public void Decentralization(string UserName, string Role)
        {
            FormsAuthentication.Initialize();

            var ticket = new FormsAuthenticationTicket(1,
                                            UserName,   //name ticket by username
                                            DateTime.Now,   // get start time
                                            DateTime.Now.AddHours(3),   // time 3 hours out
                                            false,  //do not save
                                            Role,  //Get the chain of permissions
                                            FormsAuthentication.FormsCookiePath);   //Get cookie path instead of name
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));  //create cookie (create name yourself, encrypt ticket information added to cookie)
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;    //check if cookies are available
            Response.Cookies.Add(cookie);     //
        }


        //thống kê doanh thu từ khi web thành lập
        public decimal Revenuestatistics()
        {
            decimal Totalrevenue = db.OrderDetails.Sum(n => n.Amount * n.Price).Value;
            return Totalrevenue;
        }
        public decimal Monthlyrevenuestatistics(int Month, int Year)
        {
            var lstOrder = db.Orders.Where(n => n.OrderDate.Value.Month == Month && n.OrderDate.Value.Year == Year);  //lấy ds đơn hàng có date tương ứng
            decimal Totalrevenue = 0;
            foreach (var item in lstOrder) //duyệt chi tiết từng đơn và tính tổng tiền
            {
                Totalrevenue += decimal.Parse(item.OrderDetails.Sum(n => n.Amount * n.Price).Value.ToString());
            }
            return Totalrevenue;
        }

        //Thống kê tổng đơn hàng
        public double OrderStatistics()
        {
            double numberoforders = db.Orders.Count();    //đếm số đơn hàng
            return numberoforders;
        }
        public double UserStatistics()
        {
            double numberofusers = db.Users.Count();    //đếm số thành viên
            return numberofusers;
        }

        public List<string> LoadQuestion()
        {
            List<string> lstQuestion = new List<string>();    //tạo list câu hỏi chứa câu hỏi
            lstQuestion.Add("What is your father's name?");
            lstQuestion.Add("Who is your favorite singer?");
            lstQuestion.Add("What is your favorite pet?");
            lstQuestion.Add("What's your hobby?");
            lstQuestion.Add("What work are you currently doing?");
            lstQuestion.Add("What is your high school?");
            lstQuestion.Add("What is your mother's year of birth?");
            lstQuestion.Add("What is your favorite movie?");
            lstQuestion.Add("What is your favorite song?");

            return lstQuestion;
        }

        //Giải phóng dung lượng biến db, để ở cuối controller
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}