using System;
using Microsoft.EntityFrameworkCore;
using OngProject.Common;
using OngProject.Core.Entities;

namespace OngProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //de esta manera la propiedad email de los usuarios sera unica y no se podra repetir
            builder.Entity<User>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            SeedRoles(builder);
            SeedContacts(builder);
            SeedOrganizations(builder);
            SeedUsers(builder);
            SeedCategorias(builder);
            builder.Seed();
        }

        public DbSet<Activities> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Member> Members { get; set; }  
        public DbSet<News> News { get; set; }   
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Role> Roles { get; set; }  
        public DbSet<Slides> Slides { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<User> Users { get; set; }

       
        private void SeedOrganizations(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Organizations>().HasData(
                    new Organizations
                    {
                        Id = i,
                        Name = "Organization " + i,
                        Image = "ImageOrganizations" + i + ".jpg",
                        Address = "Address for Organization " + i,
                        Phone = 381 + i,
                        Email = "Email for Organization " + i,
                        WelcomeText = "WelcomeText for Organization " + i,
                        AboutUsText = "AboutUsText for Organization " + i,
                        CreatedAt = DateTime.Now,
                        FacebookUrl = "https://www.facebook.com/" + i,
                        InstagramUrl = "https://www.instagram.com/" + i,
                        LinkedinUrl = "https://www.linkedin.com/in/" + i
                    }
                );
            }
        }

        private void SeedContacts(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Contacts>().HasData(
                    new Contacts
                    {
                        Id = i,
                        Name = "Contact " + i,
                        Phone = 381 + i,
                        Email = "Email for contact " + i,
                        Message = "Message from contact " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            for (int i = 1; i <= 2; i++)
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = i,
                        FirstName = "User" + i,
                        LastName = "LastName for user " + i,
                        Email = "Email for user " + i,
                        Password = Encrypt.GetSHA256("123456"),
                        Photo = "Photo for user " + i,
                        RoleId = 1,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Administrator",
                        Description = "Description User Admin",
                        CreatedAt = DateTime.Now
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Standard",
                        Description = "Description User Standard"
                    },
                    new Role
                    {
                        Id = 3,
                        Name = "Regular",
                        Description = "Description User Regular"
                    }                    
                );
        }

        private void SeedCategorias(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 4,
                    Name = "Ayuda niños sin hogar",
                    Image = "",
                    Description = "El ONG ayudo a muchos niños"
                },
                new Category
                {
                    Id = 1,
                    Name = "Ayuda Persona con VIH",
                    Image = "",
                    Description = "El ONG ayudo a Personas con VIH"
                },
                new Category
                {
                    Id = 2,
                    Name = "Ayuda a Persona con discapacidad",
                    Image = "",
                    Description = "El ONG ayudo a mucha gente con discapacidad"
                },
                new Category
                {
                    Id = 3,
                    Name = "Ayuda a mujer embarazadas",
                    Image = "",
                    Description = "Ayuda para  nujeres en su momento de gestacion"
                });
        }
    }
}

