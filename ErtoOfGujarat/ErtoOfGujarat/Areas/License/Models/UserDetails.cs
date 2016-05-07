using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErtoOfGujarat.Areas.License.Models
{
    public class UserDetails
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string gender { get; set; }
        public string DOB { get; set; }
        public string BloodGroup { get; set; }
        public string Orphan { get; set; }
        public decimal SecondaryPhoneNumber { get; set; }
        public string SecondaryEmailId { get; set; }
        public string fatherName { get; set; }
        public string MotherName { get; set; }
        public string GardianName { get; set; }
        public string PermentAddress { get; set; }
        public string PermentCity { get; set; }
        public string PermentPincode { get; set; }
        public string LivingThere { get; set; }
        public string PresentAddress { get; set; }
        public string PresentCity { get; set; }
        public string PresentPincode { get; set; }
        public string Duration { get; set; }
    }
}