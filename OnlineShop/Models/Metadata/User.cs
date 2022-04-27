using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models//phải cùng namespace với class trong Model
{
    [MetadataTypeAttribute(typeof(UserMetadata))] //liên kết class tv này với class tv trong Model (vì nếu update Model.edmx thì validation trong class tv của model sẽ mất hết)
    public partial class User
    {
        internal sealed class UserMetadata
        {
            public int IDUser { get; set; }

            [DisplayName("UserName")]
            [StringLength(100, ErrorMessage = "UserName should not exceed 100 characters.")]
            [Required(ErrorMessage = "Please enter UserName {0}.")]
            public string UserName { get; set; }

            [DisplayName("Full name")]
            [StringLength(50, ErrorMessage = "Name should not exceed 50 characters.")]
            [Required(ErrorMessage = "Please enter {0}.")]
            public string FullName { get; set; }

            [DisplayName("Address")]
            [Required(ErrorMessage = "Please enter {0}.")]
            public string Address { get; set; }

            [DisplayName("Email")]
            [Required(ErrorMessage = " Please enter the address {0}.")]
            [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "{0} is not valid.")]
            public string Email { get; set; }

            [DisplayName("Phone number")]
            [Required(ErrorMessage = "Please enter {0}.")]
            [StringLength(10, ErrorMessage = "{0} is not valid.")]
            public string PhoneNumber { get; set; }

            [DisplayName("Question")]
            public string Question { get; set; }

            [DisplayName("Answer")]
            [Required(ErrorMessage = "Please enter {0}.")]
            public string Answer { get; set; }
            [DisplayName("User type code")]
            public int? IDUsertype { get; set; }

            [DisplayName("Password")]
            [Required(ErrorMessage = "Password")]
            public string Password { get; set; }

        }
    }
}