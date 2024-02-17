using Contacts.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data
{
    public class ContactsDbContext : IdentityDbContext
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
            : base(options)
        {
            

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUserContact>()
                .HasKey(uc => new { uc.ApplicationUserId, uc.ContactId});

            //modelBuilder.Entity<ApplicationUserContact>()
            //    .HasOne(e => e.Event)
            //    .WithMany(e => e.EventsParticipants)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
               .Entity<Contact>()
               .HasData(new Contact()
               {
                   Id = 1,
                   FirstName = "Bruce",
                   LastName = "Wayne",
                   PhoneNumber = "+359881223344",
                   Address = "Gotham City",
                   Email = "imbatman@batman.com",
                   Website = "www.batman.com"
               });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ApplicationUserContact> ApplicationUsersContacts { get; set; }
    }
}