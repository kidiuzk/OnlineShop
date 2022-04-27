using OnlineShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class SearchController : Controller
    {
        Entities db = new Entities();
        // GET: Search
        [HttpGet]
        public ActionResult SearchResults(string skeyword, int? page)
        {
            //Phân trang
            if (Request.HttpMethod != "GET")
                page = 1;
            //tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            //tìm kiếm theo tên sản phẩm
            //dùng phương thức Contains
            var lstProduct = db.Products.Where(n => n.ProductName.Contains(skeyword));
            ViewBag.keyword = skeyword;

            return View(lstProduct.OrderBy(n => n.ProductName).ToPagedList(PageNumber, PageSize));
        }
        [HttpPost]
        public ActionResult GetSearchKeywords(string skeyword)
        {
            //gọi về hàm get tìm kiếm
            return RedirectToAction("SearchResults", new {@skeyword = skeyword });
        }
    }
}