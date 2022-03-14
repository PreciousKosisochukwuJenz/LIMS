using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.DataConnection
{
    public class DatabaseEntities : DbContext
    {
        public DatabaseEntities() : base("name=DatabaseEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseEntities, JenzHealth.DAL.Migrations.Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ApplicationSettingsRecord> ApplicationSettings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<RevenueDepartment> RevenueDepartments { get; set; }
        public virtual DbSet<ServiceDepartment> ServiceDepartments { get; set; }
        public virtual DbSet<Priviledge> Priviledges { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<Waiver> Waivers { get; set; }
        public virtual DbSet<PartPayment> PartPayments { get; set; }
        public virtual DbSet<DepositeCollection> DepositeCollections { get; set; }
        public virtual DbSet<CashCollection> CashCollections { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<Specimen> Specimens { get; set; }
        public virtual DbSet<AntiBiotic> AntiBiotics { get; set; }
        public virtual DbSet<Organism> Organisms { get; set; }
        public virtual DbSet<ServiceParameter> ServiceParameters { get; set; }
        public virtual DbSet<ServiceParameterSetup> ServiceParameterSetups { get; set; }
        public virtual DbSet<ServiceParameterRangeSetup> ServiceParameterRangeSetups { get; set; }
        public virtual DbSet<SpecimenCollection> SpecimenCollections { get; set; }
        public virtual DbSet<SpecimenCollectionCheckList> SpecimenCollectionCheckLists { get; set; }
        public virtual DbSet<TemplatedLabPreparation> TemplatedLabPreparations { get; set; }
 
    }
}
