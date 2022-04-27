using OnlineShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        Entities db = new Entities();
        //xây dựng trang xem chi tiết
        // GET: Product/ProductDetail/1
        public ActionResult ProductDetail(int? id, string nameproduct)
        {
            //check tham số truyền vào có null ko
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //nếu ko thì truy xuất csdl lấy ra sp với id tương ứng
            Product product = db.Products.SingleOrDefault(n => n.ProductID == id&& n.Status == false);   //trả về null nếu ko có id nào tương ứng 
            if (product == null)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }

            return View(product);
        }

        //xây dựng action load sp theo mã loại sp và mã nsx
        public ActionResult Product(int? CategoryID, int? ProducerID, int? page)
        {

            //check tham số truyền vào có null ko
            if (CategoryID == null || ProducerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load sp theo 2 tiêu chí là mã loại sp và mã nsx
            var lstProduct = db.Products.Where(n => n.CategoryID == CategoryID && n.ProducerID == ProducerID);
            if (lstProduct.Count() == 0)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.CategoryID = CategoryID;
            ViewBag.ProducerID = ProducerID;


            return View(lstProduct.OrderBy(n => n.ProductID).ToPagedList(PageNumber, PageSize));
        }

        //xây dựng action load sp theo mã loại sp
        public ActionResult Category(int? CategoryID, int? page)
        {
            //check tham số truyền vào có null ko
            if (CategoryID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load sp theo 2 tiêu chí là mã loại sp và mã nsx
            var lstProduct = db.Products.Where(n => n.CategoryID == CategoryID);
            if (lstProduct.Count() == 0)
            {
                //thông báo nếu ko thấy sp này
                return HttpNotFound();
            }
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.CategoryID = CategoryID;

            return View(lstProduct.OrderBy(n => n.ProductID).ToPagedList(PageNumber, PageSize));
        }

        [ChildActionOnly]
        public ActionResult ProductPartial()
        {
            return PartialView();
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