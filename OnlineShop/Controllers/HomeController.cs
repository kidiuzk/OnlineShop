using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

namespace OnlineShop.Controllers
{

    public class HomeController : Controller
    {
        Entities db = new Entities();

        // GET: Home/Index
        public ActionResult Index()
        {
            //Lần lượt tạo các viewbag để lấy list sp từ csdl
            //List laptop mới
            var lstLTM = db.Products.Where(n => n.CategoryID == 3 && n.New == 1 && n.Status == false).ToList();
            //Gán vào viewbag
            ViewBag.ListLTM = lstLTM;
            return View();
        }

        public ActionResult MenuPartial()
        {
            //Lấy ra 1 lst sanpham và truyền trực tiếp vào partial
            var lstProduct = db.Products;

            return PartialView(lstProduct);
        }

        [HttpGet]
        public ActionResult Register()
        {
            //đặt trùng tên viewbag giống trong bảng
            ViewBag.Question = new SelectList(LoadQuestion());  //gắn các câu hỏi vào viewbag để hiển thị lên view
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)    //dùng post để truyền data lên csdl, dùng biến user trong model thay formcollection
        {
            ViewBag.Question = new SelectList(LoadQuestion());  //lưu câu hỏi đã chọn trong dropdownlist vào csdl
            //Kiểm tra captcha hợp lệ
            if (this.IsCaptchaValid("Invalid Captcha!"))   //nếu captcha hợp lệ
            {
                if (ModelState.IsValid)
                {
                    user.IDUsertype = 3;
                    ViewBag.Notification = "More success";
                    //Thêm khách hàng vào csdl
                    db.Users.Add(user);  //sau khi lấy được các thuộc tính trong biến tv qua các textbox thì truyền tv vào dbset Customers
                    //Lưu thay đổi
                    db.SaveChanges();   //lấy data từ dbset chuyển vào csdl
                }
                return View();
            }
            ViewBag.Notification = "Wrong Captcha Code";
            return View();
        }

        //Load câu hỏi để đưa vào dropdownlist
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

        //xd action dang nhap
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {

            //check username and password
            //khai báo string để lấy chuỗi vì username password có thể có ktu đặc biệt nên phải khai báo string
            string username = f["txtUserName"].ToString();   //lấy chuỗi trong txtUserName
            string password = f["txtPassword"].ToString();    //lấy chuỗi trong txtPassword
            //so sánh với tk và mk trong csdl
            //truy vấn kiểm tra đăng nhập lấy thông tin user
            User user = db.Users.SingleOrDefault(n => n.UserName == username && n.Password == password);      //compare with username and password in database
            if (user != null) //nếu đăng nhập đúng
            {
                //lấy ra list quyền của user tương ứng với userType
                var lstRole = db.UserType_Role.Where(n => n.IDUsertype == user.IDUsertype);   //lấy ra list quyền tương ứng UserType_Role
                //duyệt list role
                string Role = ""; //tạo biến string chứa mã quyền
                if (lstRole.Count() != 0)
                {
                    foreach (var item in lstRole)   //duyệt list quyền
                    {
                        Role += item.Role.RoleID + ",";  //lấy quyền trong bản chi tiết quyền và loại thành viên
                        //"Admin,Manage,Login,"
                        //xét role theo kiểu tăng dần trong database //dấu phẩy dùng để phân tách quyền của từng thằng trong UserTypeRole
                    }
                    Role = Role.Substring(0, Role.Length - 1); //Cắt dấu , (Chuỗi sau khi cắt:"Admin,Manage,Login")

                    Decentralization(user.UserName.ToString(), Role); // xử lý phương thức phân quyền

                    Session["UserName"] = user;
                    return Content(@"<script>window.location.reload()</script>");   //script used to reload the page after successful login
                }
            }
            return Content("Incorrect account or password.");
        }




        public ActionResult Logout()
        {
            Session["UserName"] = null; //set session to null

            FormsAuthentication.SignOut();  //delete cookies

            return RedirectToAction("Index");
        }

        public ActionResult Authorizationword()
        {
            return View();
        }

        public void Decentralization(string UserName, string Role)
        {
            FormsAuthentication.Initialize(); // khởi tạo FormsAuthentication

            var ticket = new FormsAuthenticationTicket(1,
                                            UserName,   //name ticket by username
                                            DateTime.Now,   // get start time
                                            DateTime.Now.AddHours(3),   // time 3 hours out
                                            false,  //remember ?
                                            Role,  //Get the chain of permissions
                                            FormsAuthentication.FormsCookiePath);   //Lấy đg dẫn cookie thay vì name
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));   //tạo cookie(tự tạo name, mã hóa thông tin ticket add vào cookie)
                                                                                                 //Encrypt mã hóa cookie
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;   //ktra cookie có chưa
            Response.Cookies.Add(cookie);     //Response yêu cầu gửi từ client đến sever
        }

        //Free up the database variable space, put it at the end of the controller
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