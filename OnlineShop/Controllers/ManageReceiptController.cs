using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin,Manage")]
    public class ManageReceiptController : Controller
    {
        Entities db = new Entities();
        // GET: ManageReceipt
        [HttpGet]
        public ActionResult ImportGoods()
        {
            ViewBag.ManufactureID = db.Manufactures;
            ViewBag.ListProduct = db.Products;
            ViewBag.ReceiptDate = DateTime.Today;

            return View();
        }
        [HttpPost]
        public ActionResult ImportGoods(Receipt model, IEnumerable<ReceiptDetail> lstModel)
        {

            ViewBag.ManufactureID = db.Manufactures;
            ViewBag.ListProduct = db.Products;
            ViewBag.ReceiptDate = DateTime.Today;

            model.ReceiptDate = ViewBag.ReceiptDate;
            model.Deleted = false;
            //Sau khi đã ktra hết dl đầu vào

            db.Receipts.Add(model);
            db.SaveChanges();   //save để lấy MaPN gán cho lst chitietpn
            Product product;
            foreach (var item in lstModel)
            {
                product = db.Products.Single(n => n.ProductID == item.ProductID);
                product.Quantity += item.ImportNumber;  //update solg tồn

                item.ReceiptID = model.ReceiptID; //gán MaPN cho all chitietpn
            }
            db.ReceiptDetails.AddRange(lstModel);
            db.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult lstProoutofstock()
        {
            //ds sp gần hết hàng với số lượng tồn bé hơn hoặc bằng 5
            var lstProduct = db.Products.Where(n => n.Status == false && n.Quantity <= 5);
            return View(lstProduct);
        }

        [HttpGet]
        public ActionResult Importorder(int? id)
        {
            ViewBag.ManufactureID = new SelectList(db.Manufactures.OrderBy(n => n.ManufactureName), "ManufactureID", "ManufactureName");

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Product product = db.Products.SingleOrDefault(n => n.ProductID == id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        public ActionResult Importorder(Receipt model, ReceiptDetail receiptdetail)
        {
            ViewBag.ManufactureID = new SelectList(db.Manufactures.OrderBy(n => n.ManufactureName), "ManufactureID", "ManufactureName", model.ManufactureID);

            model.ReceiptDate = DateTime.Now;
            model.Deleted = false;
            db.Receipts.Add(model);
            db.SaveChanges();   //save để lấy MaPN gán cho lst chitietpn

            receiptdetail.ReceiptID = model.ReceiptID;
            Product product = db.Products.Single(n => n.ProductID == receiptdetail.ProductID);
            product.Quantity += receiptdetail.ImportNumber;
            db.ReceiptDetails.Add(receiptdetail);
            db.SaveChanges();

            return View(product);
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