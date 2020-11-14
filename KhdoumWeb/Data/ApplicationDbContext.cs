using System;
using System.Collections.Generic;
using System.Text;
using KhdoumWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KhdoumWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemsCategories { get; set; }
        public DbSet<ItemSupCategory> ItemsSupCategories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberItem> MembersItems { get; set; }
        public DbSet<Raiting> Raitings { get; set; }
        public DbSet<Role> Roless { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ItemCategory
            modelBuilder.Entity<ItemCategory>()
                .HasKey(IS => new { IS.ItemId, IS.CategoryId });


            modelBuilder.Entity<ItemCategory>()
                .HasOne(IS => IS.Item)
                .WithMany(i => i.ItemsCategories)
                .HasForeignKey(IS => IS.ItemId)
                .IsRequired();

            modelBuilder.Entity<ItemCategory>()
                .HasOne(IS => IS.Category)
                .WithMany(s => s.ItemsCategories)
                .HasForeignKey(IS => IS.CategoryId)
                .IsRequired();

            //ItemSupCategory
            modelBuilder.Entity<ItemSupCategory>()
                .HasKey(IS => new { IS.ItemId, IS.SubCategoryId });


            modelBuilder.Entity<ItemSupCategory>()
                .HasOne(IS => IS.Item)
                .WithMany(i => i.ItemsSupCategories)
                .HasForeignKey(IS => IS.ItemId)
                .IsRequired();

            modelBuilder.Entity<ItemSupCategory>()
                .HasOne(IS => IS.SubCategory)
                .WithMany(s => s.ItemsSupCategories)
                .HasForeignKey(IS => IS.SubCategoryId)
                .IsRequired();

            //MemberItem
            modelBuilder.Entity<MemberItem>()
               .HasKey(IS => new { IS.ItemId, IS.MemberId });

            modelBuilder.Entity<MemberItem>()
                .HasOne(IS => IS.Item)
                .WithMany(i => i.MembersItems)
                .HasForeignKey(IS => IS.ItemId)
                .IsRequired();

            modelBuilder.Entity<MemberItem>()
                .HasOne(IS => IS.Member)
                .WithMany(s => s.MembersItems)
                .HasForeignKey(IS => IS.MemberId)
                .IsRequired();

            //MemberRole
            modelBuilder.Entity<MemberRole>()
                .HasKey(IS => new { IS.RoleId, IS.MemberId });


            modelBuilder.Entity<MemberRole>()
                .HasOne(IS => IS.Role)
                .WithMany(i => i.MembersRoles)
                .HasForeignKey(IS => IS.RoleId)
                .IsRequired();

            modelBuilder.Entity<MemberRole>()
                .HasOne(IS => IS.Member)
                .WithMany(s => s.MembersRoles)
                .HasForeignKey(IS => IS.MemberId)
                .IsRequired();



            base.OnModelCreating(modelBuilder);




        }


    }
}
