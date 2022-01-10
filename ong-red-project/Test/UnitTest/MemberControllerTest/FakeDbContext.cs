using System;
using Microsoft.EntityFrameworkCore;
using OngProject.Infrastructure.Data;

namespace Test.UnitTest.MemberTest
{
    public class FakeDbContext
    {
        public static ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new ApplicationDbContext(options);

            return context;
        }


    }
}