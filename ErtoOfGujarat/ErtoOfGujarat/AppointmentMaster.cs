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
    
    public partial class AppointmentMaster
    {
        public int appointmentId { get; set; }
        public int id { get; set; }
        public Nullable<int> timeOfAppointment { get; set; }
    
        public virtual AppointmentTimeMaster AppointmentTimeMaster { get; set; }
        public virtual ErtoMaster ErtoMaster { get; set; }
    }
}
