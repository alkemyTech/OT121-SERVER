using Microsoft.EntityFrameworkCore;
using OngProject.Infrastructure.Data;

namespace Test.Configuration
{
    public static class ApplicationDbContextInMemory
    {
        public static ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Tests")
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}