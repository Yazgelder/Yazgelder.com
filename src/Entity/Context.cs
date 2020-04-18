using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yazgelder.Entity.Models;

namespace Yazgelder.Entity
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<BoardDecisions> BoardDecisions { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Dues> Dues { get; set; }
        public DbSet<Link> Link { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<MemberRequest> MemberRequest { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategories> NewsCategories { get; set; }
        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<NewsTags> NewsTags { get; set; }
        public DbSet<ProjectProposal> ProjectProposal { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TallySheet> TallySheet { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<SendMesageList> SendMesageList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoardDecisions>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Title).HasMaxLength(250);
                x.Property(y => y.Body).HasMaxLength(4000);
            });

            modelBuilder.Entity<Categories>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<Contact>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
                x.Property(y => y.Surname).HasMaxLength(250);
                x.Property(y => y.EMail).HasMaxLength(250);
                x.Property(y => y.Message).HasMaxLength(4000);
            });

            modelBuilder.Entity<Dues>(x =>
            {
                x.HasKey(y => y.Id);
            });

            modelBuilder.Entity<Events>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Title).HasMaxLength(250);
                x.Property(y => y.Title).HasMaxLength(4000);
            });

            modelBuilder.Entity<Link>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
                x.Property(y => y.LinkName).HasMaxLength(250);
            });

            modelBuilder.Entity<Member>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
                x.Property(y => y.Surname).HasMaxLength(250);
                x.Property(y => y.TCIdentity).HasMaxLength(11);
                x.Property(y => y.Telephone).HasMaxLength(19);
                x.Property(y => y.Nationality).HasMaxLength(250);
                x.Property(y => y.MotherName).HasMaxLength(250);
                x.Property(y => y.FatherName).HasMaxLength(250);
                x.Property(y => y.Gender).HasMaxLength(250);
                x.Property(y => y.Job).HasMaxLength(250);
                x.Property(y => y.Email).HasMaxLength(250);
                x.Property(y => y.PlaceOfResidence).HasMaxLength(550);
                x.Property(y => y.Reference).HasMaxLength(250);
                x.Property(y => y.Reference2).HasMaxLength(250);
            });

            modelBuilder.Entity<MemberRequest>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
                x.Property(y => y.Surname).HasMaxLength(250);
                x.Property(y => y.Telephone).HasMaxLength(19);
                x.Property(y => y.Gender).HasMaxLength(250);
                x.Property(y => y.Job).HasMaxLength(250);
                x.Property(y => y.Email).HasMaxLength(250);
                x.Property(y => y.Reference).HasMaxLength(250);
                x.Property(y => y.Reference2).HasMaxLength(250);
            });

            modelBuilder.Entity<News>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Title).HasMaxLength(250);
                x.Property(y => y.LinkName).HasMaxLength(250);
                x.Property(y => y.Body).HasMaxLength(4000);
                x.Property(y => y.Sender).HasMaxLength(250);
            });

            modelBuilder.Entity<NewsCategories>(x =>
            {
                x.HasKey(y => y.Id);

                x.HasOne(d => d.News)
                  .WithMany(p => p.NewsCategoriesList)
                  .HasForeignKey(d => d.NewsId)
                  .HasConstraintName("FK_NewsId_NewsCategoriesList");

                x.HasOne(d => d.Categories)
                  .WithMany(p => p.NewsCategoriesList)
                  .HasForeignKey(d => d.CategoriesId)
                  .HasConstraintName("FK_CategoriesId_NewsCategoriesList");
            });

            modelBuilder.Entity<Newsletter>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Mail).HasMaxLength(250);
                x.Property(y => y.IpAdress).HasMaxLength(250);
            });

            modelBuilder.Entity<NewsTags>(x =>
            {
                x.HasKey(y => y.Id);

                x.HasOne(d => d.News)
                  .WithMany(p => p.NewsTagsList)
                  .HasForeignKey(d => d.NewsId)
                  .HasConstraintName("FK_NewsId_NewsTagsListList");

                x.HasOne(d => d.Tag)
                  .WithMany(p => p.NewsTagsList)
                  .HasForeignKey(d => d.TagId)
                  .HasConstraintName("FK_TagId_NewsTagsList");
            });

            modelBuilder.Entity<ProjectProposal>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.SenderMail).HasMaxLength(250);
                x.Property(y => y.SenderName).HasMaxLength(250);
                x.Property(y => y.SenderSurname).HasMaxLength(250);
                x.Property(y => y.ProjectName).HasMaxLength(250);
                x.Property(y => y.ProjectExplanation).HasMaxLength(4000);
            });

            modelBuilder.Entity<Tag>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Name).HasMaxLength(250);
                x.Property(y => y.LinkName).HasMaxLength(250);
            });

            modelBuilder.Entity<TallySheet>(x =>
            {
                x.HasKey(y => y.Id);
            });

            modelBuilder.Entity<User>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.UserName).HasMaxLength(250);
                x.Property(y => y.Password).HasMaxLength(250);
            });

            modelBuilder.Entity<Pictures>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Picture).HasMaxLength(250);
                x.Property(y => y.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<Projects>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.Title).HasMaxLength(250);
                x.Property(y => y.Content).HasMaxLength(4000);
                x.Property(y => y.Technology).HasMaxLength(500);
            });

            modelBuilder.Entity<SendMesageList>(x =>
            {
                x.HasKey(y => y.Id);
                x.Property(y => y.To).HasMaxLength(250);
                x.Property(y => y.Title).HasMaxLength(250);
                x.Property(y => y.Message).HasMaxLength(4000);
            });
        }
    }
}