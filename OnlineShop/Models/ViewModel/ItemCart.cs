using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models.ViewModel
{
    public class ItemCart
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal IntoMoney { get; set; }
        public string Img { get; set; }
        public ItemCart() { }
        //constructor theo id (dùng cho trường hợp chỉ có sl=1)
        public ItemCart(int iProductID)
        {
            using (Entities db = new Entities())
            {
                this.ProductID = iProductID;
                Product product = db.Products.Single(n => n.ProductID == iProductID);
                this.ProductName = product.ProductName;
                this.Img = product.Img;
                this.Price = product.Price.Value;  //kiểu decimal dùng value để lấy gtri
                this.Amount = 1;
                this.IntoMoney = Price * Amount;
            }
        }
        public ItemCart(int iProductID, int amount)
        {
            using (Entities db = new Entities())
            {
                this.ProductID = iProductID;
                Product product = db.Products.Single(n => n.ProductID == iProductID);
                this.ProductName = product.ProductName;
                this.Img = product.Img;
                this.Price = product.Price.Value;
                this.Amount = amount;
                this.IntoMoney = Price * Amount;
            }
        }
    }
}