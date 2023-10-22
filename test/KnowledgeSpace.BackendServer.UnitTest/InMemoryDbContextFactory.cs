using KnowledgeSpace.BackendServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.UnitTest
{
    public class InMemoryDbContextFactory
    {
        public static ApplicationDbcontext GetApplicationDbContext(string Name = "InMemoryApplicationDatabase")
        {
            var options = new DbContextOptionsBuilder<ApplicationDbcontext>().UseInMemoryDatabase(databaseName: Name).Options;
            var dbContext = new ApplicationDbcontext(options);
            if (dbContext != null)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            return dbContext;
        }
    }
}
