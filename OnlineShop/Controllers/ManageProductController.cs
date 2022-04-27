using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin,Manage")]
    public class ManageProductController : Controller
    {
        Entities db = new Entities();
        // GET: ManageProduct
        public ActionResult Index()
        {

            return View(db.Products.Where(n => n.Status == false).OrderBy(n => n.ProducerID));
        }

        [HttpGet]
        public ActionResult Create()
        {
            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sx
            ViewBag.ManufactureID = new SelectList(db.Manufactures.OrderBy(n => n.ManufactureID), "ManufactureID", "ManufactureName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryID), "CategoryID", "CategoryName");
            ViewBag.ProducerID = new SelectList(db.Producers.OrderBy(n => n.ProducerID), "ProducerID", "ProducerName");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase Img, HttpPostedFileBase Img1, HttpPostedFileBase Img2, HttpPostedFileBase Img3, HttpPostedFileBase Img4)
        {
            if (ModelState.IsValid)
            {   
                //ktra hình ảnh tồn tại trong csdl
                #region UpImg
                if (Img != null)
                {
                    if (Img.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Img.FileName);  //Lấy tên hình
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), fileName);   //lấy hình đưa vào folder ImgSP

                        //lấy hình đưa vào folder
                        Img.SaveAs(path);
                        product.Img = fileName;  //lưu vào sp
                    }
                }

                if (Img1 != null)
                {
                    if (Img1.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Img1.FileName);  //Lấy tên hình
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), fileName);  //lấy hình đưa vào folder ImgSP

                        //lấy hình đưa vào folder
                        Img1.SaveAs(path);
                        product.Img1 = fileName;  //lưu vào sp
                    }
                }
                if (Img2 != null)
                {
                    if (Img2.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Img2.FileName);  //Lấy tên hình
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), fileName);  //lấy hình đưa vào folder ImgSP

                        //lấy hình đưa vào folder
                        Img2.SaveAs(path);
                        product.Img2 = fileName;  //lưu vào sp
                    }
                }
                if (Img3 != null)
                {
                    if (Img3.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Img3.FileName);  //Lấy tên hình
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), fileName);  //lấy hình đưa vào folder ImgSP

                        //lấy hình đưa vào folder
                        Img3.SaveAs(path);
                        product.Img3 = fileName;  //lưu vào sp
                    }
                }
                if (Img4 != null)
                {
                    if (Img4.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Img4.FileName);  //Lấy tên hình
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), fileName);  //lấy hình đưa vào folder ImgSP

                        //lấy hình đưa vào folder
                        Img4.SaveAs(path);
                        product.Img4 = fileName;  //lưu vào sp
                    }
                }

                #endregion

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sx
            ViewBag.ManufactureID = new SelectList(db.Manufactures.OrderBy(n => n.ManufactureName), "ManufactureID", "ManufactureName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryID), "CategoryID", "CategoryName");
            ViewBag.ProducerID = new SelectList(db.Producers.OrderBy(n => n.ProducerID), "ProducerID", "ProducerName"); ;
            return View("Create");
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
            Product product  = db.Products.SingleOrDefault(n => n.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sx
            ViewBag.ManufactureID = createlistManufacture(product.ManufactureID.Value);
            ViewBag.CategoryID = createlistCategory(product.CategoryID.Value);
            ViewBag.ProducerID = createlistProducer(product.ProducerID.Value);

            return View(product);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Price,UpdateDay,Configuration,Description,Quantity,Views,votes,Comments,Purchases,New,ManufactureID,ProducerID,CategoryID,Status")] Product product)
        {

            if (ModelState.IsValid)
            {
                Product proDuct = db.Products.Find(product.ProductID);
                TryUpdateModel(proDuct, new string[] { "ProductID", "ProductName", "Price", "UpdateDay", "Configuration", "Description", "Quantity", "Views", "votes",
                    "Comments", "Purchases", "New", "ManufactureID", "ProducerID", "CategoryID", "Status" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ManufactureID", "ManufactureName", product.ManufactureID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", product.ProducerID);

            return View(product);
        }

        [HttpGet]
        public ActionResult UploadImg(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImg(int id, HttpPostedFileBase Img, HttpPostedFileBase Img1, HttpPostedFileBase Img2, HttpPostedFileBase Img3, HttpPostedFileBase Img4)
        {
            Product product = db.Products.Find(id);

            if (Img != null)
            {
                string path = Server.MapPath("~/Content/Img");
                string Ten = null;
                Img.SaveAs(path + Img.FileName);
                Ten = Img.FileName;

                if (!string.IsNullOrEmpty(product.Img))
                {
                    string pathAndFname = Server.MapPath($"~/Content/Img/{product.Img}");
                    if (System.IO.File.Exists(pathAndFname))
                        System.IO.File.Delete(pathAndFname);
                }
                product.Img = Ten;
            }

            if (Img1 != null)
            {
                string path = Server.MapPath("~/Content/Img/");
                string Ten = null;
                Img1.SaveAs(path + Img1.FileName);
                Ten = Img1.FileName;

                if (!string.IsNullOrEmpty(product.Img1))
                {
                    string pathAndFname = Server.MapPath($"~/Content/Img/{product.Img1}");
                    if (System.IO.File.Exists(pathAndFname))
                        System.IO.File.Delete(pathAndFname);
                }
                product.Img1 = Ten;
            }

            if (Img2 != null)
            {
                string path = Server.MapPath("~/Content/Img/");
                string Ten = null;
                Img2.SaveAs(path + Img2.FileName);
                Ten = Img2.FileName;

                if (!string.IsNullOrEmpty(product.Img2))
                {
                    string pathAndFname = Server.MapPath($"~/Content/Img/{product.Img2}");
                    if (System.IO.File.Exists(pathAndFname))
                        System.IO.File.Delete(pathAndFname);
                }
                product.Img2 = Ten;
            }

            if (Img3 != null)
            {
                string path = Server.MapPath("~/Content/Img/");
                string Ten = null;
                Img3.SaveAs(path + Img3.FileName);
                Ten = Img3.FileName;

                if (!string.IsNullOrEmpty(product.Img3))
                {
                    string pathAndFname = Server.MapPath($"~/Content/Img/{product.Img3}");
                    if (System.IO.File.Exists(pathAndFname))
                        System.IO.File.Delete(pathAndFname);
                }
                product.Img3 = Ten;
            }


            if (Img4 != null)
            {
                string path = Server.MapPath("~/Content/Img/");
                string Ten = null;
                Img4.SaveAs(path + Img4.FileName);
                Ten = Img4.FileName;

                if (!string.IsNullOrEmpty(product.Img4))
                {
                    string pathAndFname = Server.MapPath($"~/Content/Img/{product.Img4}");
                    if (System.IO.File.Exists(pathAndFname))
                        System.IO.File.Delete(pathAndFname);
                }
                product.Img4 = Ten;
            }

            db.SaveChanges();

            //upload thành công
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
            Product product = db.Products.SingleOrDefault(n => n.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ManufactureID", "ManufactureName", product.ManufactureID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProducerID = new SelectList(db.Producers, "ProducerID", "ProducerName", product.ProducerID);

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //lấy sp cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.SingleOrDefault(n => n.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private SelectList createlistManufacture(int IDselect = 0)
        {
            var items = db.Manufactures.Select(p => new { p.ManufactureID,Information = p.ManufactureName }).ToList();
            var result = new SelectList(items, "ManufactureID", "Information", selectedValue: IDselect);
            return result;
        }
        private SelectList createlistProducer(int IDselect = 0)
        {
            var items = db.Producers.Select(p => new { p.ProducerID, Information = p.ProducerName }).ToList();
            var result = new SelectList(items, "ProducerID", "Information", selectedValue: IDselect);
            return result;
        }
        private SelectList createlistCategory(int IDselect = 0)
        {
            var items = db.Categories.Select(p => new { p.CategoryID, Information = p.CategoryName }).ToList();
            var result = new SelectList(items, "CategoryID", "Information", selectedValue: IDselect);
            return result;
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