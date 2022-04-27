using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin,Manage")]
    public class ManageProducerController : Controller
    {
        Entities db = new Entities();
        // GET: ManageProducer
        public ActionResult Index()
        {
            return View(db.Producers.OrderBy(n => n.ProducerID));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Producer producer)
        {
            db.Producers.Add(producer);
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
            Producer producer = db.Producers.SingleOrDefault(n => n.ProducerID == id);
            if (producer == null)
            {
                return HttpNotFound();
            }

            return View(producer);
        }
        [HttpPost]
        public ActionResult Edit(Producer producer)
        {
            //if (ModelState.IsValid)
            db.Entry(producer).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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
            Producer producer = db.Producers.SingleOrDefault(n => n.ProducerID == id);
            if (producer == null)
            {
                return HttpNotFound();
            }

            return View(producer);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Producer producer = db.Producers.SingleOrDefault(n => n.ProducerID == id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            db.Producers.Remove(producer);
            db.SaveChanges();

            return RedirectToAction("Index");
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