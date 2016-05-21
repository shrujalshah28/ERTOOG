using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErtoOfGujarat.Areas.License.Models
{
    public class UserDetails
    {
        public int RequestId { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Birth Date")]
        public string DOB { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Display(Name = "Orphan")]
        public string Orphan { get; set; }
        [Display(Name = "Phone Number")]
        public decimal SecondaryPhoneNumber { get; set; }
        [Display(Name = "Email Id")]
        public string SecondaryEmailId { get; set; }
        [Display(Name = "Father Name")]
        public string fatherName { get; set; }
        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }
        [Display(Name = "Gardian Name")]
        public string GardianName { get; set; }
        [Display(Name = "Address")]
        public string PermentAddress { get; set; }
        [Display(Name = "City")]
        public string PermentCity { get; set; }
        [Display(Name = "Pincode")]
        public string PermentPincode { get; set; }
        [Display(Name = "Live There")]
        public string LivingThere { get; set; }
        [Display(Name = "Address")]
        public string PresentAddress { get; set; }
        [Display(Name = "City")]
        public string PresentCity { get; set; }
        [Display(Name = "Pincode")]
        public string PresentPincode { get; set; }
        [Display(Name = "Duration")]
        public string Duration { get; set; }
    }
}