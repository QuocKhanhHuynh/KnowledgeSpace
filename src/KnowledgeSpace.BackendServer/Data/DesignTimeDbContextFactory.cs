using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KnowledgeSpace.BackendServer.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbcontext>
    {
        public ApplicationDbcontext CreateDbContext(string[] args)
        {
            var enviromentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{enviromentName}.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbcontext>();
            var connecttionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connecttionString);
            return new ApplicationDbcontext(builder.Options);
        }
    }
}