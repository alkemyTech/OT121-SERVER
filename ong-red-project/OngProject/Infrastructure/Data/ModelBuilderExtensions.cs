
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
                    FirstName = "Juan",
                    LastName = "Benitez",
                    Email = "juan1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now                
                },
                new User {
                    FirstName = "Ramiro",
                    LastName = "Bueno",
                    Email = "ramiro1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Ernesto",
                    LastName = "Gamarra",
                    Email = "ernesto1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Florencia",
                    LastName = "Gutierrez",
                    Email = "florencia1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Miguel",
                    LastName = "Perez",
                    Email = "miguel1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Elvira",
                    LastName = "Sanchez",
                    Email = "elvira1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Carlos",
                    LastName = "Espíndola",
                    Email = "carlos1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Julieta",
                    LastName = "Bermudez",
                    Email = "julieta1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Ricardo",
                    LastName = "Ramos",
                    Email = "ricardo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User {
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
                    FirstName = "Diego",
                    LastName = "De La Vega",
                    Email = "diego1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Luis",
                    LastName = "Miguel",
                    Email = "luis1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Pepe",
                    LastName = "Arjona",
                    Email = "pepe1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Mirtha",
                    LastName = "Lopez",
                    Email = "mirtha1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Julián",
                    LastName = "Weich",
                    Email = "julian1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Teresa",
                    LastName = "Parodi",
                    Email = "teresa1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Jimena",
                    LastName = "Ayala",
                    Email = "jimena1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Paul",
                    LastName = "Samson",
                    Email = "paul1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
                    FirstName = "Rodrigo",
                    LastName = "Bueno",
                    Email = "rodrigo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User {
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
