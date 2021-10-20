namespace ClearMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class ClearModelContext : DbContext
    {
        public ClearModelContext() : base("name=ClearModel") { }

        public virtual DbSet<Activities> Activities { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<ProjectDetailsTypes> ProjectDetailsTypes { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserTypes> UserTypes { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Refuse> Refuses { get; set; }
        public virtual DbSet<Firma> Firmas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contacts>()
                .Property(e => e.GSM)
                .IsFixedLength();

            modelBuilder.Entity<Messages>()
                .Property(e => e.Sender)
                .IsFixedLength();

            modelBuilder.Entity<ProjectDetailsTypes>()
                .HasMany(e => e.ProjectDetails)
                .WithOptional(e => e.ProjectDetailsTypes)
                .HasForeignKey(e => e.ProjectDetailTypeId);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.Contacts)
                .WithOptional(e => e.Projects)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.ProjectDetails)
                .WithOptional(e => e.Projects)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.GSM)
                .IsFixedLength();

            modelBuilder.Entity<Firma>()
                .HasMany(e => e.Activities)
                .WithOptional(e => e.Firma)
                .HasForeignKey(e => e.FirmaId);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Firma>()
                .HasMany(e => e.News)
                .WithOptional(e => e.Firma)
                .HasForeignKey(e => e.FirmaId);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.ProjectDetails)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.PersonId);

            modelBuilder.Entity<Customers>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.Customers)
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.UserDetails)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.UserId);
        }
        
    }
}
