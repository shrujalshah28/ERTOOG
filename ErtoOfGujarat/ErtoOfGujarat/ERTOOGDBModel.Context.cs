﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ERTOOGDBEntities : DbContext
    {
        public ERTOOGDBEntities()
            : base("name=ERTOOGDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppointmentDateMaster> AppointmentDateMasters { get; set; }
        public virtual DbSet<AppointmentMaster> AppointmentMasters { get; set; }
        public virtual DbSet<AppointmentTimeMaster> AppointmentTimeMasters { get; set; }
        public virtual DbSet<AuthorizePerson> AuthorizePersons { get; set; }
        public virtual DbSet<ContectMaster> ContectMasters { get; set; }
        public virtual DbSet<DigitalInfoMaster> DigitalInfoMasters { get; set; }
        public virtual DbSet<ErtoMaster> ErtoMasters { get; set; }
        public virtual DbSet<ExternalIdentityMaster> ExternalIdentityMasters { get; set; }
        public virtual DbSet<GardianMaster> GardianMasters { get; set; }
        public virtual DbSet<LicenseMaster> LicenseMasters { get; set; }
        public virtual DbSet<PermentAddressMaster> PermentAddressMasters { get; set; }
        public virtual DbSet<PersentAddressMaster> PersentAddressMasters { get; set; }
        public virtual DbSet<ResponceMaster> ResponceMasters { get; set; }
        public virtual DbSet<TestResultMaster> TestResultMasters { get; set; }
        public virtual DbSet<vanueMaster> vanueMasters { get; set; }
        public virtual DbSet<VerificationMaster> VerificationMasters { get; set; }
    }
}
