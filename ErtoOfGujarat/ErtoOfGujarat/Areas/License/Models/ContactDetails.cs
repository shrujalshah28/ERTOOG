using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErtoOfGujarat.Areas.License.Models
{
    public class ContactDetails
    {
        public int requestId;
        [Display(Name ="Age")]
        public int Age;
        [Display(Name ="Phone Number")]
        public decimal phoneNo;
        [Display(Name ="Email Id")]
        public string emailId;
    }
}