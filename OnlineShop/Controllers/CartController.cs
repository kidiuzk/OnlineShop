using OnlineShop.Models;
using OnlineShop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        Entities db = new Entities();
        // GET: Cart
        public ActionResult CartPartial()
        {
            if (TotalAmount() == 0) //ktra soluong giỏ hàng
            {
                ViewBag.TotalAmount = 0;
                ViewBag.TotalPrice = 0;
                return PartialView();
            }
            //Hiển thị tổng tiền và sl sp lên trên icon giỏ hàng
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalPrice = TotalPrice();

            return PartialView();
        }

        //Thêm giỏ hàng thông thường ko ajax
        public ActionResult AddtoCart(int ProductID, string strURL)
        {
            //ktra sp có tồn tại trong csdl ko
            Product product = db.Products.SingleOrDefault(n => n.ProductID == ProductID);  //lấy sp với masp tương ứng
            if (product == null)  //nếu lấy sai masp
            {
                //Trang đường dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy giỏ hàng
            List<ItemCart> lstCart = GetCart();

            //trường hợp nếu 1 sp đã tồn tại trong giỏ hàng
            ItemCart productCheck = lstCart.SingleOrDefault(n => n.ProductID == ProductID);  //ktra sp có trong lst đã tạo hay ko
            if (productCheck != null)
            {
                //ktra số lg tồn trc khi cho kh mua hàng
                if (product.Quantity < productCheck.Amount)
                    return View("Notification");
                //nếu sp đã có trong list thì khi thêm vào giỏ hàng sẽ tăng số lượng lên
                productCheck.Amount++;
                //và đơn giá sẽ tăng theo giá sp * sl tương ứng
                productCheck.IntoMoney = productCheck.Amount * productCheck.Price;
                return Redirect(strURL);
            }

            //nếu chưa tồn tại thì thêm vào list
            ItemCart itemCart = new ItemCart(ProductID);
            if (product.Quantity < itemCart.Amount) //ktra số lg tồn trc khi cho kh mua hàng
                return View("Notification");
            lstCart.Add(itemCart);
            return Redirect(strURL);
        }

        // GET: GioHang
        //Trang xem giỏ hàng
        public ActionResult ViewCart()
        {
            //lấy giỏ hàng đã đc tạo
            List<ItemCart> lstCart = GetCart();
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalPrice = TotalPrice();

            return View(lstCart);    //đưa list vào view
        }

        //chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult EditCart(int ProductID)
        {
            //ktra session giỏ hàng có tồn tại ko
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");   //quay về trang chủ
            }
            //ktra sp có tồn tại trong csdl ko
            Product product = db.Products.SingleOrDefault(n => n.ProductID == ProductID);
            if (product == null)
            {
                //Trang đường dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy list giỏ hàng từ session
            List<ItemCart> lstCart = GetCart();

            //ktra xem sp đó có tồn tại trong giỏ hàng hay ko
            ItemCart productCheck = lstCart.SingleOrDefault(n => n.ProductID == ProductID);
            if (productCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Lấy list giỏ hàng tạo giao diện
            ViewBag.Cart = lstCart;

            //nếu tồn tại thì
            return View(productCheck);
        }

        //Cập nhật giỏ hàng
        [HttpPost]
        public ActionResult UpdateCart(ItemCart itemCart)
        {
            //ktra số lượng tồn sau khi sửa
            Product productCheck = db.Products.Single(n => n.ProductID == itemCart.ProductID);
            if (productCheck.Quantity < itemCart.Amount)
            {
                return View("Notification");
            }

            //update số lg trong session giỏ hàng
            //bc1: Lấy list giỏ hàng từ sesssion giỏ hàng
            List<ItemCart> lstCart = GetCart();
            //bc2: lấy sp cần update từ trong list giỏ hàng
            ItemCart itemCartUpdate = lstCart.Find(n => n.ProductID == itemCart.ProductID);  //pt find dùng để tìm các trường mong muốn
            //bc3: update lại số lg và thành tiền
            itemCartUpdate.Amount = itemCart.Amount;
            itemCartUpdate.IntoMoney = itemCartUpdate.Amount * itemCartUpdate.Price;

            return RedirectToAction("ViewCart");
        }

        //Xóa giỏ hàng
        public ActionResult DeleteCart(int ProductID)
        {
            //ktra session giỏ hàng có tồn tại ko
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //ktra sp có tồn tại trong csdl ko
            Product product = db.Products.SingleOrDefault(n => n.ProductID == ProductID);
            if (product == null)
            {
                //Trang đường dẫn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemCart> lstCart = GetCart();
            //ktra xem sp đó có tồn tại trong giỏ hàng hay ko
            ItemCart productCheck = lstCart.SingleOrDefault(n => n.ProductID == ProductID);
            if (productCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa item trong giỏ hàng
            lstCart.Remove(productCheck);

            return RedirectToAction("ViewCart");
        }

        //chức năng đặt hàng
        public ActionResult Order(Customer customer)
        {
            //ktra session giỏ hàng có tồn tại ko
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //kiểm tra từng loại khách hàng
            Customer customer1 = new Customer();
            if (Session["UserName"] == null)     //nếu session rỗng thì là khách vãng lai
            {
                //thêm khách hàng vào bảng khách hàng đối vs khách vãng lai
                customer1 = customer; //biến kh được truyền dữ liệu khi nhập thông tin vào form
                db.Customers.Add(customer1);   //thêm vào bảng kh
                db.SaveChanges();   //lưu vào db và tăng mãkh
            }
            else
            {
                //đối vs kh là thành viên
                User user = (User)Session["UserName"]; //tạo biến tv lấy dữ liệu tù session
                customer1.CustomerName = user.UserName; //  gắn dữ liệu vào biến khách hàng
                customer1.Address = user.Address;
                customer1.Email = user.Email;
                customer1.Phone = user.PhoneNumber;
                db.Customers.Add(customer1);   //thêm vào bảng kh
                db.SaveChanges();   //lưu vào db và tăng mãkh
            }

            //thêm đơn hàng
            Order order = new Order();

            order.CustomerID = int.Parse(customer1.CustomerID.ToString()); //thêm vào makh lấy từ kh
            order.OrderDate = DateTime.Now; //lấy ngày hiện tại trên hệ thống
            order.Deliverystatus = false;
            order.Paid = false;
            order.Endow = 0;
            order.Cancelled = false;
            order.Deleted = false;
            db.Orders.Add(order);    //thêm vào bảng dondathang giá trị ddh
            db.SaveChanges();   //update bảng đơn đặt hàng, và tạo mã ddh dùng cho chitietddh

            //thêm chi tiết đơn đặt hàng
            List<ItemCart> lstCart = GetCart(); //lấy list giỏ hàng
            foreach (var item in lstCart)  //chạy vòng lập để lấy thông tin của từng sp trong giỏ hàng đưa vào chitietddh
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderID = order.OrderID; // lấy mã tù ddh đã tạo
                orderDetail.ProductID = item.ProductID;
                orderDetail.ProductName = item.ProductName;
                orderDetail.Amount = item.Amount;
                orderDetail.Price = item.Price;
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges(); //update vào bảng chi tiết đơn đặt hàng

            Session["Cart"] = null;  //sau khi thêm vào ddh thì giỏ hàng sẽ trống
            return RedirectToAction("ViewCart");
        }



        #region Methods
        //Lấy giỏ hàng
        public List<ItemCart> GetCart()
        {
            //nếu giỏ hàng đã tồn tại
            List<ItemCart> lstCart = Session["Cart"] as List<ItemCart>; //lưu giỏ hàng vào session giohang để quản lý
            if (lstCart == null)
            {
                //Nếu session giỏ hàng ko tồn tại thì khởi tạo giỏ hàng
                lstCart = new List<ItemCart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        // tính tổng số lg
        public double TotalAmount()
        {
            //lấy giỏ hàng
            List<ItemCart> lstCart = Session["Cart"] as List<ItemCart>;
            if (lstCart == null)  //nếu chưa có list giỏ hàng thì trả về gtri = 0
            {
                return 0;
            }
            return lstCart.Sum(n => n.Amount); //trả về tổng số lượng của list giỏ hàng
        }

        //tính tổng tiền
        public decimal TotalPrice()
        {
            //lấy giỏ hàng
            List<ItemCart> lstCart = Session["Cart"] as List<ItemCart>;
            if (lstCart == null) //nếu chưa có list giỏ hàng thì trả về gtri = 0
            {
                return 0;
            }
            return lstCart.Sum(n => n.IntoMoney);    //trả về tổng tiền của list giỏ hàng
        }
        #endregion









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