
using System;
using Microsoft.EntityFrameworkCore;
using OngProject.Common;
using OngProject.Core.Entities;

namespace OngProject.Infrastructure.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                
                //Standard Users

                new User {
                    Id = 11,
                    FirstName = "Juan",
                    LastName = "Benitez",
                    Email = "juan1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now                
                },
                new User {
                    Id = 12,
                    FirstName = "Ramiro",
                    LastName = "Bueno",
                    Email = "ramiro1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 13,
                    FirstName = "Ernesto",
                    LastName = "Gamarra",
                    Email = "ernesto1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 14,
                    FirstName = "Florencia",
                    LastName = "Gutierrez",
                    Email = "florencia1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 15,
                    FirstName = "Miguel",
                    LastName = "Perez",
                    Email = "miguel1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 16,
                    FirstName = "Elvira",
                    LastName = "Sanchez",
                    Email = "elvira1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 17,
                    FirstName = "Carlos",
                    LastName = "Espíndola",
                    Email = "carlos1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 18,
                    FirstName = "Julieta",
                    LastName = "Bermudez",
                    Email = "julieta1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 19,
                    FirstName = "Ricardo",
                    LastName = "Ramos",
                    Email = "ricardo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 20,
                    FirstName = "Oscar",
                    LastName = "Alfonsín",
                    Email = "oscar1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                
                //Regular Users
                
                new User {
                    Id = 21,
                    FirstName = "Diego",
                    LastName = "De La Vega",
                    Email = "diego1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 22,
                    FirstName = "Luis",
                    LastName = "Miguel",
                    Email = "luis1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 23,
                    FirstName = "Pepe",
                    LastName = "Arjona",
                    Email = "pepe1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 24,
                    FirstName = "Mirtha",
                    LastName = "Lopez",
                    Email = "mirtha1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 25,
                    FirstName = "Julián",
                    LastName = "Weich",
                    Email = "julian1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 26,
                    FirstName = "Teresa",
                    LastName = "Parodi",
                    Email = "teresa1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 27,
                    FirstName = "Jimena",
                    LastName = "Ayala",
                    Email = "jimena1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 28,
                    FirstName = "Paul",
                    LastName = "Samson",
                    Email = "paul1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 29,
                    FirstName = "Rodrigo",
                    LastName = "Bueno",
                    Email = "rodrigo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Id = 30,
                    FirstName = "Cristina",
                    LastName = "Quiroga",
                    Email = "cris1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                }

            );
        }       
    }
}
