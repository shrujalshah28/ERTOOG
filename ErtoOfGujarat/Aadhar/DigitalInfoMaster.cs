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
    public partial class DigitalInfoMaster
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public byte[] photo { get; set; }
        [DataMember]
        public byte[] lThumb { get; set; }
        [DataMember]
        public byte[] rThumb { get; set; }
        [DataMember]
        public byte[] lFingers { get; set; }
        [DataMember]
        public byte[] rFingers { get; set; }
        [DataMember]
        public byte[] signature { get; set; }
    
        public virtual AadharMaster AadharMaster { get; set; }
    }
}
