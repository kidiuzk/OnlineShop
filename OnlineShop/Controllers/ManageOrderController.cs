using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin,Manage")]
    public class ManageOrderController : Controller
    {
        Entities db = new Entities();

        // GET: ManageOrder


        public ActionResult Unpaid()
        {
            //lấy ds đơn hàng chưa duyệt
            var lst = db.Orders.Where(n => n.Paid == false).OrderBy(n => n.OrderDate);
            return View(lst);
        }

        public ActionResult notdelivery()
        {
            //lấy ds đơn hàng chưa giao
            var lstOnotdelivered = db.Orders.Where(n => n.Deliverystatus == false && n.Paid == true).OrderBy(n => n.OrderDate);
            return View(lstOnotdelivered);
        }

        public ActionResult deliveredpaid()
        {
            //lấy ds đơn hàng đã giao và thanh toán
            var lstOnotdelivered = db.Orders.Where(n => n.Deliverystatus == true && n.Paid == true).OrderBy(n => n.OrderDate);
            return View(lstOnotdelivered);
        }

        [HttpGet]
        public ActionResult BrowseOrders(int? id)
        {
            //ktra id hợp lệ
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Order model = db.Orders.SingleOrDefault(n => n.OrderID == id);
            //ktra đơn hàng có tồn tại ko
            if (model == null)
            {
                return HttpNotFound();
            }
            //hiển thị ds chitietdonhang 
            var lstOrderDetail = db.OrderDetails.Where(n => n.OrderID == id);
            ViewBag.listOrderDetail = lstOrderDetail;
            ViewBag.CustomerName = model.Customer.CustomerName;
            return View(model);
        }

        [HttpPost]
        public ActionResult BrowseOrders(Order order)
        {
            Order orderUpdate = db.Orders.Single(n => n.OrderID == order.OrderID);    //lấy dl của đơn hàng trên
            orderUpdate.Paid = order.Paid;
            orderUpdate.Deliverystatus = order.Deliverystatus;
            db.SaveChanges();

            var lstOrderDetail = db.OrderDetails.Where(n => n.OrderID == order.OrderID);
            ViewBag.listOrderDetail = lstOrderDetail;

            return View(orderUpdate);
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