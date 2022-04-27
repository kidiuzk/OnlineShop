using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DecentralizationController : Controller
    {
        Entities db = new Entities();
        // GET: Decentralization
        public ActionResult Index()
        {
            return View(db.UserTypes.OrderBy(n => n.UsertypeName));
        }

        [HttpGet]
        public ActionResult Decentralization(int? id)
        {
            //lấy mã loại tv dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UserType usertype = db.UserTypes.SingleOrDefault(n => n.IDUsertype == id);
            if (usertype == null)
                return HttpNotFound();
            //lấy ra danh sách các quyền để load lên check box
            ViewBag.RoleID = db.Roles; //lấy ds quyền
            //b1: lấy ra các quyền thuộc loại user dựa vào bảng UserType_Role
            ViewBag.UserType_Role = db.UserType_Role.Where(n => n.IDUsertype == id);  //lấy ds loại của tv đó
            return View(usertype);
        }
        [HttpPost]
        public ActionResult Decentralization(int? idusertype, IEnumerable<UserType_Role> lstDecentralization)
        {
            //trường hợp: nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //b1: xóa những quyền cũ thuộc loại tv đó
            var lstauthorized = db.UserType_Role.Where(n => n.IDUsertype == idusertype);
            if (lstauthorized.Count() != 0)
            {
                db.UserType_Role.RemoveRange(lstauthorized);
                db.SaveChanges();
            }

            if (lstauthorized != null)
            {
                foreach (var item in lstDecentralization)
                {
                    item.IDUsertype = int.Parse(idusertype.ToString());
                    db.UserType_Role.Add(item);   //nếu đc check thì add data vào bảng phanquyen
                }
                db.SaveChanges();
            }
            //ktra list ds quyền dc check


            return RedirectToAction("Index");
        }
    }
}