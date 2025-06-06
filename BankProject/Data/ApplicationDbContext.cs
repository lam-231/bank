using BankProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            builder.Entity<User>()
                .HasOne(u => u.Card)
                .WithOne(c => c.User)
                .HasForeignKey<Card>(c => c.UserId);

            builder.Entity<Contact>()
                .HasIndex(c => new { c.UserId, c.ContactCardNumber })
                .IsUnique(false);
        }
    }
}
