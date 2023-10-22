using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class CommandsControllerTest
    {
        private ApplicationDbcontext _context { set; get; }
        public CommandsControllerTest()
        {
            _context = InMemoryDbContextFactory.GetApplicationDbContext("CommandMemory");
        }
        public void ShouldCreateInstance_NotNull_Success()
        {
            var commandsController = new CommandsController(_context);
            Assert.NotNull(commandsController);
        }

        public async Task GetCommands_HasData_Success()
        {
            _context.Commands.AddRange(
                new Command()
                {
                    Id = "Test1",
                    Name = "Test1",
                },
                new Command()
                {
                    Id = "Test2",
                    Name = "Test2",
                }
            );
            await _context.SaveChangesAsync();
            var commandsController = new CommandsController(_context);
            var result = await commandsController.GetCommands();
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var commands = okResult.Value as List<CommandViewModel>;
            Assert.True(commands.Count() > 0);
        }
    }
}
