using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    [MetadataTypeAttribute(typeof(ProductMetadata))]
    public partial class Product
    {
        internal sealed class ProductMetadata
        {
            public int ProductID { get; set; }

            [DisplayName("Product's name")]
            public string ProductName { get; set; }

            [DisplayName("Price")]
            public decimal? Price { get; set; }

            [DisplayName("Update day")]
            public DateTime? UpdateDay { get; set; }

            [DisplayName("Configuration")]
            public string Configuration { get; set; }

            [DisplayName("Picture")]
            public string Img { get; set; }

            [DisplayName("Inventory Quantity")]
            public int? Quantity { get; set; }

            [DisplayName("View")]
            public int? Views { get; set; }

            [DisplayName("Votes")]
            public int? votes { get; set; }

            [DisplayName("Comments")]
            public int? Comments { get; set; }

            [DisplayName("Number of Purchases")]
            public int? Purchases { get; set; }

            [DisplayName("New product")]
            public int? New { get; set; }

            [DisplayName("Manufacture code")]
            public int? ManufactureID { get; set; }

            [DisplayName("Producer Code")]
            public int? ProducerID { get; set; }

            [DisplayName("Product Type Code")]
            public int? CategoryID { get; set; }

            [DisplayName("Status")]
            public bool? Status { get; set; }

            [DisplayName("Picture 1")]
            public string Img1 { get; set; }

            [DisplayName("Picture 2")]
            public string Img2 { get; set; }

            [DisplayName("Picture 3")]
            public string Img3 { get; set; }

            [DisplayName("Picture 4")]
            public string Img4 { get; set; }

            [DisplayName("Description")]
            public string Description { get; set; }
        }
    }
}