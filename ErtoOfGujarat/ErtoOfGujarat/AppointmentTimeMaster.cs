//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErtoOfGujarat
{
    using System;
    using System.Collections.Generic;
    
    public partial class AppointmentTimeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppointmentTimeMaster()
        {
            this.AppointmentMasters = new HashSet<AppointmentMaster>();
        }
    
        public int pkey { get; set; }
        public int date { get; set; }
        public int timeSlot { get; set; }
        public Nullable<int> totalCount { get; set; }
    
        public virtual AppointmentDateMaster AppointmentDateMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppointmentMaster> AppointmentMasters { get; set; }
    }
}
