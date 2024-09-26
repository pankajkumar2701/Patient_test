using Microsoft.EntityFrameworkCore;
using Patient_test.Entities;

namespace Patient_test.Data
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class Patient_testContext : DbContext
    {
        /// <summary>
        /// Configures the database connection options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database connection.</param>
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=PatientTest.database.windows.net;Initial Catalog=T117034_Patient_test;Persist Security Info=True;user id=PatientTest;password=Pankaj@27;Integrated Security=false;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;");
        }

        /// <summary>
        /// Configures the model relationships and entity mappings.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRole>().HasKey(a => a.Id);
            modelBuilder.Entity<UserToken>().HasKey(a => a.Id);
            modelBuilder.Entity<RoleEntitlement>().HasKey(a => a.Id);
            modelBuilder.Entity<Entity>().HasKey(a => a.Id);
            modelBuilder.Entity<Tenant>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(a => a.Id);
            modelBuilder.Entity<Role>().HasKey(a => a.Id);
            modelBuilder.Entity<Patient>().HasKey(a => a.Id);
            modelBuilder.Entity<Gender>().HasKey(a => a.Id);
            modelBuilder.Entity<AgeUnit>().HasKey(a => a.Id);
            modelBuilder.Entity<Contact>().HasKey(a => a.Id);
            modelBuilder.Entity<Location>().HasKey(a => a.Id);
            modelBuilder.Entity<Membership>().HasKey(a => a.Id);
            modelBuilder.Entity<Title>().HasKey(a => a.Id);
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Country>().HasKey(a => a.Id);
            modelBuilder.Entity<City>().HasKey(a => a.Id);
            modelBuilder.Entity<State>().HasKey(a => a.Id);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.RoleId_Role).WithMany().HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.UserId_User).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<UserToken>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserToken>().HasOne(a => a.UserId_User).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.RoleId_Role).WithMany().HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.EntityId_Entity).WithMany().HasForeignKey(c => c.EntityId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Entity>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<User>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.TenantId_Tenant).WithMany().HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.CreatedBy_User).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Role>().HasOne(a => a.UpdatedBy_User).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Patient>().HasOne(a => a.Gender_Gender).WithMany().HasForeignKey(c => c.Gender);
            modelBuilder.Entity<Patient>().HasOne(a => a.AgeUnit_AgeUnit).WithMany().HasForeignKey(c => c.AgeUnit);
            modelBuilder.Entity<Patient>().HasOne(a => a.ReferredById_Contact).WithMany().HasForeignKey(c => c.ReferredById);
            modelBuilder.Entity<Patient>().HasOne(a => a.LocationId_Location).WithMany().HasForeignKey(c => c.LocationId);
            modelBuilder.Entity<Patient>().HasOne(a => a.MembershipId_Membership).WithMany().HasForeignKey(c => c.MembershipId);
            modelBuilder.Entity<Patient>().HasOne(a => a.Title_Title).WithMany().HasForeignKey(c => c.Title);
            modelBuilder.Entity<Patient>().HasOne(a => a.PatientAddressId_Address).WithMany().HasForeignKey(c => c.PatientAddressId);
            modelBuilder.Entity<Contact>().HasOne(a => a.TitleId_Title).WithMany().HasForeignKey(c => c.TitleId);
            modelBuilder.Entity<Contact>().HasOne(a => a.StateId_State).WithMany().HasForeignKey(c => c.StateId);
            modelBuilder.Entity<Contact>().HasOne(a => a.CityId_City).WithMany().HasForeignKey(c => c.CityId);
            modelBuilder.Entity<Contact>().HasOne(a => a.CountryId_Country).WithMany().HasForeignKey(c => c.CountryId);
            modelBuilder.Entity<Location>().HasOne(a => a.CountryId_Country).WithMany().HasForeignKey(c => c.CountryId);
            modelBuilder.Entity<Location>().HasOne(a => a.StateId_State).WithMany().HasForeignKey(c => c.StateId);
            modelBuilder.Entity<Location>().HasOne(a => a.CityId_City).WithMany().HasForeignKey(c => c.CityId);
            modelBuilder.Entity<Address>().HasOne(a => a.CountryId_Country).WithMany().HasForeignKey(c => c.CountryId);
            modelBuilder.Entity<Address>().HasOne(a => a.CityId_City).WithMany().HasForeignKey(c => c.CityId);
            modelBuilder.Entity<Address>().HasOne(a => a.StateId_State).WithMany().HasForeignKey(c => c.StateId);
            modelBuilder.Entity<City>().HasOne(a => a.CountryId_Country).WithMany().HasForeignKey(c => c.CountryId);
            modelBuilder.Entity<City>().HasOne(a => a.StateId_State).WithMany().HasForeignKey(c => c.StateId);
            modelBuilder.Entity<State>().HasOne(a => a.CountryId_Country).WithMany().HasForeignKey(c => c.CountryId);
        }

        /// <summary>
        /// Represents the database set for the UserInRole entity.
        /// </summary>
        public DbSet<UserInRole> UserInRole { get; set; }

        /// <summary>
        /// Represents the database set for the UserToken entity.
        /// </summary>
        public DbSet<UserToken> UserToken { get; set; }

        /// <summary>
        /// Represents the database set for the RoleEntitlement entity.
        /// </summary>
        public DbSet<RoleEntitlement> RoleEntitlement { get; set; }

        /// <summary>
        /// Represents the database set for the Entity entity.
        /// </summary>
        public DbSet<Entity> Entity { get; set; }

        /// <summary>
        /// Represents the database set for the Tenant entity.
        /// </summary>
        public DbSet<Tenant> Tenant { get; set; }

        /// <summary>
        /// Represents the database set for the User entity.
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Represents the database set for the Role entity.
        /// </summary>
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// Represents the database set for the Patient entity.
        /// </summary>
        public DbSet<Patient> Patient { get; set; }

        /// <summary>
        /// Represents the database set for the Gender entity.
        /// </summary>
        public DbSet<Gender> Gender { get; set; }

        /// <summary>
        /// Represents the database set for the AgeUnit entity.
        /// </summary>
        public DbSet<AgeUnit> AgeUnit { get; set; }

        /// <summary>
        /// Represents the database set for the Contact entity.
        /// </summary>
        public DbSet<Contact> Contact { get; set; }

        /// <summary>
        /// Represents the database set for the Location entity.
        /// </summary>
        public DbSet<Location> Location { get; set; }

        /// <summary>
        /// Represents the database set for the Membership entity.
        /// </summary>
        public DbSet<Membership> Membership { get; set; }

        /// <summary>
        /// Represents the database set for the Title entity.
        /// </summary>
        public DbSet<Title> Title { get; set; }

        /// <summary>
        /// Represents the database set for the Address entity.
        /// </summary>
        public DbSet<Address> Address { get; set; }

        /// <summary>
        /// Represents the database set for the Country entity.
        /// </summary>
        public DbSet<Country> Country { get; set; }

        /// <summary>
        /// Represents the database set for the City entity.
        /// </summary>
        public DbSet<City> City { get; set; }

        /// <summary>
        /// Represents the database set for the State entity.
        /// </summary>
        public DbSet<State> State { get; set; }
    }
}