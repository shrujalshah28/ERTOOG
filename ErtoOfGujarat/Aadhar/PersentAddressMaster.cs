//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aadhar
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    
    [Serializable]
    [DataContract]
    public partial class PersentAddressMaster
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string mainAddress { get; set; }
        [DataMember]
        public string nearByAddress { get; set; }
        [DataMember]
        public string optionalAddress { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public Nullable<decimal> pincode { get; set; }
        [DataMember]
        public Nullable<int> duration { get; set; }
    
        public virtual AadharMaster AadharMaster { get; set; }
    }
}
