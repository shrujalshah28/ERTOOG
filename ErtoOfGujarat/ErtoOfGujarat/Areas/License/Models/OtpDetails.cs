using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErtoOfGujarat.Areas.License.Models
{
    public class OtpDetails
    {
        public int requestId;
        [Display(Name ="Mobile OTP")]
        public int mobileOTP;
        [Display(Name = "Email OTP")]
        public int emailOTP;
    }
}