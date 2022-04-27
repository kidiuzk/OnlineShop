using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageRoleController : Controller
    {
        Entities db = new Entities();
        // GET: ManageRole
        public ActionResult Index()
        {
            return View(db.Roles.OrderBy(n => n.RoleName));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Role role)
        {
            db.Roles.Add(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == "")
            {
                Response.StatusCode = 404;
                return null;
            }
            Role role = db.Roles.SingleOrDefault(n => n.RoleID == id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "RoleID,RoleName")] Role role)
        {
            //if (ModelState.IsValid)
            Role q = db.Roles.Find(role.RoleID);
            TryUpdateModel(q, new string[] { "RoleID", "RoleName" });       //ko dc phep doi ma quyen
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == "")
            {
                Response.StatusCode = 404;
                return null;
            }
            Role role = db.Roles.SingleOrDefault(n => n.RoleID == id);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]   //trùng tên
        public ActionResult ConfirmDelete(string id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.SingleOrDefault(n => n.RoleID == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            db.Roles.Remove(role);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}