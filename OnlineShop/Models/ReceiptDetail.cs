//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReceiptDetail
    {
        public int ReceiptDetailID { get; set; }
        public Nullable<int> ReceiptID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> ImportPrice { get; set; }
        public Nullable<int> ImportNumber { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}
