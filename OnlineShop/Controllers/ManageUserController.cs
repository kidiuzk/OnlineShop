using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        Entities db = new Entities();
        // GET: ManageUser
        public ActionResult Index()
        {
            return View(db.Users.OrderBy(n => n.IDUser));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Question = new SelectList(LoadQuestion());
            ViewBag.IDUsertype = new SelectList(db.UserTypes.OrderBy(n => n.IDUsertype), "IDUsertype", "UsertypeName");

            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            ViewBag.Question = new SelectList(LoadQuestion());
            ViewBag.IDUsertype = new SelectList(db.UserTypes.OrderBy(n => n.IDUsertype), "IDUsertype", "UsertypeName");

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            User user = db.Users.SingleOrDefault(n => n.IDUser == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Question = new SelectList(LoadQuestion(), selectedValue: user.Question);
            ViewBag.IDUsertype = createusertypelist(user.IDUsertype.Value);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User uSer = db.Users.Find(user.IDUser);
                TryUpdateModel(uSer, new string[] { "IDUser", "UserName", "Password", "FullName", "Address", "Email",
                                                        "PhoneNumber", "Question", "Answer", "IDUsertype" });

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Question = new SelectList(user.Question);
            ViewBag.IDUsertype = new SelectList(db.UserTypes, "IDUsertype", "UsertypeName", user.IDUsertype);
            return View(user);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            User user = db.Users.SingleOrDefault(n => n.IDUser == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Question = new SelectList(LoadQuestion(), selectedValue: user.Question);
            ViewBag.IDUsertype = createusertypelist(user.IDUsertype.Value);
            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            User user = db.Users.SingleOrDefault(n => n.IDUser == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        private SelectList createusertypelist(int IDselect = 0)
        {
            var items = db.UserTypes.Select(p => new { p.IDUsertype, Information = p.UsertypeName }).ToList();
            var result = new SelectList(items, "IDUsertype", "Information", selectedValue: IDselect);
            return result;
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