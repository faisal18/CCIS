using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using Entities;
using System.Configuration;

namespace DAL.DataModel
{

    //[DbConfigurationType(typeof(DataContextConfiguration))]

    public class DALDbContext : DbContext
    {
        public DbSet<AddressLocations> AddressLocations { get; set; }

        public DbSet<Applications> Applications { get; set; }
        public DbSet<CallerInformation> CallerInformation { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<InquiryDetails> InquiryDetails { get; set; }
        public DbSet<ItemTypes> ItemTypes { get; set; }
        public DbSet<MaintenanceLogging> MLogging { get; set; }
        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<PayerType> payerTypes { get; set; }
        public DbSet<Payers> payerLookups { get; set; }
        public DbSet<PersonInformation> PersonInformation { get; set; }
        public DbSet<ProviderType> providerTypes { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<SLADeclarations> SLADeclarations { get; set; }
        public DbSet<SLAExecutionLog> SLAExecutionLogs { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<SystemUser> SystemUser { get; set; }
        public DbSet<TicketAttachment> TicketAttachment { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketInformation> TicketInformation { get; set; }
        public DbSet<UserApplication> UserApplication { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<ApplicationProp> ApplicationProps { get; set; }
        public DbSet<TicketApplicationProp> TicketApplicationProps { get; set; }
        public DbSet<TicketRelation> TicketRelations { get; set; }
        public DbSet<JiraTicket> JiraTickets { get; set; }
        public DbSet<JiraTicketComments> jiraTicketComments { get; set; }





        public DALDbContext() : base("name=DefaultDBConnection")
        {
            try
            {
                var ensureDLLIsCopied = SqlProviderServices.Instance;
                this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                //  Database.SetInitializer<DALDbContext>(new CreateDatabaseIfNotExists<DALDbContext>());
                /// for ignore DB changes 
                Database.SetInitializer<DALDbContext>(null);


                //  this.Database.Initialize.
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
            }
        }
        


        public static DALDbContext Create()
        {
            try
            {
                return new DALDbContext();
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return null;
            }
        }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SLAExecutionLog>().HasRequired<SLADeclarations>(s => s.SLADeclarations_FK)
            //    .WithMany(x => x.SLAExecutionLogs_FK)
            //    .HasForeignKey<int>(x => x.SLAID);


            modelBuilder.Entity<SLAExecutionLog>()
               .Property(e => e.ActionComments)
               .IsUnicode(false);

        }


    }
}
