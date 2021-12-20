
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
                    CreatedAt = new DateTime(2021,12,15)                
                },
                new User {
                    Id = 12,
                    FirstName = "Ramiro",
                    LastName = "Bueno",
                    Email = "ramiro1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 13,
                    FirstName = "Ernesto",
                    LastName = "Gamarra",
                    Email = "ernesto1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 14,
                    FirstName = "Florencia",
                    LastName = "Gutierrez",
                    Email = "florencia1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 15,
                    FirstName = "Miguel",
                    LastName = "Perez",
                    Email = "miguel1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 16,
                    FirstName = "Elvira",
                    LastName = "Sanchez",
                    Email = "elvira1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 17,
                    FirstName = "Carlos",
                    LastName = "Espíndola",
                    Email = "carlos1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 18,
                    FirstName = "Julieta",
                    LastName = "Bermudez",
                    Email = "julieta1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 19,
                    FirstName = "Ricardo",
                    LastName = "Ramos",
                    Email = "ricardo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 20,
                    FirstName = "Oscar",
                    LastName = "Alfonsín",
                    Email = "oscar1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 2,
                    CreatedAt = new DateTime(2021,12,15)
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
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 22,
                    FirstName = "Luis",
                    LastName = "Miguel",
                    Email = "luis1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 23,
                    FirstName = "Pepe",
                    LastName = "Arjona",
                    Email = "pepe1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 24,
                    FirstName = "Mirtha",
                    LastName = "Lopez",
                    Email = "mirtha1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 25,
                    FirstName = "Julián",
                    LastName = "Weich",
                    Email = "julian1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 26,
                    FirstName = "Teresa",
                    LastName = "Parodi",
                    Email = "teresa1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 27,
                    FirstName = "Jimena",
                    LastName = "Ayala",
                    Email = "jimena1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 28,
                    FirstName = "Paul",
                    LastName = "Samson",
                    Email = "paul1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 29,
                    FirstName = "Rodrigo",
                    LastName = "Bueno",
                    Email = "rodrigo1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                },
                new User {
                    Id = 30,
                    FirstName = "Cristina",
                    LastName = "Quiroga",
                    Email = "cris1@gmail.com",
                    Password = Encrypt.GetSHA256("1234"),
                    Photo = "",
                    RoleId = 3,
                    CreatedAt = new DateTime(2021,12,15)
                }
            );

            modelBuilder.Entity<Activities>().HasData(
                //Activities
                new Activities {
                    Id = 1,
                    Name = "Apoyo Escolar para el nivel Primario",
                    Content = "El espacio de apoyo escolar es el corazón del área educativa. Se realizan los talleres de lunes a jueves de 10 a 12 horas y de 14 a 16 horas en el contraturno, Los sábados también se realiza el taller para niños y niñas que asisten a la escuela doble turno. Tenemos un espacio especial para los de 1er grado 2 veces por semana ya que ellos necesitan atención especial!",
                    Image = "",
                    CreatedAt = new DateTime(2021,12,12)
                },

                new Activities {
                    Id = 2,
                    Name = "Apoyo Escolar Nivel Secundaria",
                    Content = "Del mismo modo que en primaria, este taller es el corazón del área secundaria. Se realizan talleres de lunes a viernes de 10 a 12 horas y de 16 a 18 horas en el contraturno. Actualmente se encuentran inscriptos en el taller 50 adolescentes entre 13 y 20 años. Aquí los jóvenes se presentan con el material que traen del colegio y una docente de la institución y un grupo de voluntarios los recibe para ayudarlos a estudiar o hacer la tarea.",
                    Image = "",
                    CreatedAt = new DateTime(2021,11,01)
                }, 

                new Activities {
                    Id = 3,
                    Name = "Tutorias",
                    Content = "Es un programa destinado a jóvenes a partir del tercer año de secundaria, cuyo objetivo es garantizar su permanencia en la escuela y construir un proyecto de vida que da sentido al colegio. El objetivo de esta propuesta es lograr la integración escolar de niños y jóvenes del barrio promoviendo el soporte socioeducativo y emocional apropiado, desarrollando los recursos institucionales necesarios a través de la articulación de nuestras intervenciones con las escuelas que los alojan, con las familias de los alumnos y con las instancias municipales, provinciales y nacionales que correspondan.",
                    Image = "",
                    CreatedAt = new DateTime(2021,02,04)
                },

                new Activities {
                    Id = 4,
                    Name = "Taller De Arte y Cuentos",
                    Content = "Taller literario y de manualidades que se realiza semanalmente.",
                    Image = "",
                    CreatedAt = new DateTime(2021,11,12)
                },

                new Activities {
                    Id = 5,
                    Name = "Paseos recreativos y educativos",
                    Content = "Estos paseos están pensados para promover la participación y sentido de pertenencia de los niños, niñas y adolescentes al área educativa.",
                    Image = "",
                    CreatedAt = new DateTime(2021,10,11)
                }
            );

            //News Test Data
            modelBuilder.Entity<News>().HasData(
                new News {
                    Id = 1,
                    Name = "Programa de medicamentos gratuitos",
                    Content = "Gracias al Gobierno provincial estaremos entregando medicamentos gratuitos a aquellas personas que padezcan VIH",
                    CategoryId = 1,
                    CreatedAt = new DateTime(2021,03,01)
                },
                new News {
                    Id = 2,
                    Name = "Taller de Idiomas",
                    Content = "Inauguramos el taller de idiomas para personas con alguna discapacidad, el mismo instruye al alumno en idiomas como Inglés y Francés.",
                    CategoryId = 2,
                    CreatedAt = new DateTime(2021,03,01)
                },
                new News {
                    Id = 3,
                    Name = "Plan de nutrición a embarazadas",
                    Content = "La desnutrición en embarazadas es un caso que se presenta frecuentemente en la comunidad. Por eso lanzamos este plan de nutrición para garantizar que la gestación se lleve a cabo de manera saludable tanto para el niño como para la madre.",
                    CategoryId = 3,
                    CreatedAt = new DateTime(2021,03,01)
                },
                new News {
                    Id = 4,
                    Name = "Asistencia en reubicación de hogar",
                    Content = "Estamos comprometidos con la comunidad para reubicar a los niños sin hogar, con esto se busca garantizarles un ambiente familiar que todo ser humano necesita.",
                    CategoryId = 4, 
                    CreatedAt = new DateTime(2021,06,01)
                }
            );

        }       
    }
}

      