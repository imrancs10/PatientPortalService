using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace SWN.MobileService.Api.Data
{
    public class OnSolveMobileContextFactory : IDesignTimeDbContextFactory<MobileServiceContext>
    {
        public MobileServiceContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MobileServiceContext>();
            builder.UseSqlServer("Server= .\\;Database=OnSolveMobile;Trusted_Connection=True;",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(OnSolveMobileContextFactory).GetTypeInfo().Assembly.GetName().Name));

            return new MobileServiceContext(builder.Options);
        }
    }
}
