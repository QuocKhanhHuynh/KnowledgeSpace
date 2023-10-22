using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class FunctionsControllerTest
    {
        private ApplicationDbcontext _context;
        public FunctionsControllerTest()
        {
            _context =  InMemoryDbContextFactory.GetApplicationDbContext("FunctionsControllerTest");
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var _functionController = new FunctionsController(_context);
            Assert.NotNull(_functionController);
        }
       
        [Fact]
        public async Task PostFunction_Validate_Success()
        {
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PostFunction(new FunctionCreateRequest()
            {
                Id = "Test",
                Name = "Test",
                Url = "/Test",
                SortOrder = 9,
                ParentId = null,
                Icon = null
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }
       
       [Fact]
       public async Task PostFunction_Validate_Failled()
       {
            _context.Functions.Add(
                new Function()
                {
                    Id = "Test",
                    Name = "Test",
                    Url = "/Test",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PostFunction(new FunctionCreateRequest()
           {
               Id = "Test",
               Name = "Test",
               Url = "/Test",
               SortOrder = 1,
               ParentId = null,
               Icon = null
           });
           Assert.NotNull(result);
           Assert.IsType<BadRequestObjectResult>(result);
       }
        
      [Fact]
      public async Task GetById_HasData_Success()
      {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Test1",
                    Name = "Test1",
                    Url = "/Test1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Test2",
                    Name = "Test2",
                    Url = "/Test2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.GetById("Test1");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var functionVM = okResult.Value as FunctionViewModel;
            Assert.Equal("Test1", functionVM.Id);
      }

        [Fact]
        public async Task GetById_HasData_Fail()
        {
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.GetById("Test1");
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
      public async Task GetFunctions_HasData_Success()
      {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Test1",
                    Name = "Test1",
                    Url = "/Test1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Test2",
                    Name = "Test2",
                    Url = "/Test2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = _functionController.GetFunctions();
          Assert.NotNull(result);
          var okResult = await result as OkObjectResult;
          var functions = okResult.Value as List<FunctionViewModel>;
          Assert.True(functions.Count() > 0);
          ;
      }

      [Fact]
      public async Task GetPaging_NoFilter_Success()
      {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Test1",
                    Name = "Test1",
                    Url = "/Test1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Test2",
                    Name = "Test2",
                    Url = "/Test2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.GetFunctionsPaging(1, 2, null);
          Assert.NotNull(result);
          var okResult = result as OkObjectResult;
          var functions = okResult.Value as Pagination<FunctionViewModel>;
           Assert.True(/*functions.TotalRecords == 2 &&*/ functions.Items.Count() == 2);
        }

      [Fact]
      public async Task GetPaging_HasFilter_Success()
      {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Test1",
                    Name = "Test1",
                    Url = "/Test1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Test2",
                    Name = "Test2",
                    Url = "/Test2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = _functionController.GetFunctionsPaging(1, 2, "Test1");
          Assert.NotNull(result);
          var okResult = await result as OkObjectResult;
          var functions = okResult.Value as Pagination<FunctionViewModel>;
          Assert.True(functions.TotalRecords == 1 && functions.Items.Count() == 1 && functions.Items.Any(x => x.Id.Contains("Test1") || x.Name.Contains("Test1") || x.Url.Contains("Test1")));
      }

      [Fact]
      public async Task PutFucntion_Validata_Success()
      {
            _context.Functions.Add(
                new Function()
                {
                    Id = "Test1",
                    Name = "Test1",
                    Url = "/Test1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PutFunction("Test1", new FunctionCreateRequest()
          {
              Id = "Test1",
              Name = "Test",
              Url = "/Test",
              SortOrder = 1,
              ParentId = null,
              Icon = null
          });
          Assert.NotNull(result);
          Assert.IsType<NoContentResult>(result);
      }

      [Fact]
      public async Task PutFunction_Validata_Failled()
      {
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PutFunction("Test", new FunctionCreateRequest()
          {
              Id = "Test",
              Name = "Test",
              Url = "/Test",
              SortOrder = 9,
              ParentId = null,
              Icon = null
          });
          Assert.NotNull(result);
          Assert.IsType<NotFoundObjectResult>(result);
        }

      [Fact]
      public async Task DeleteFunction_Validata_Success()
      {
            _context.Functions.Add(
               new Function()
               {
                   Id = "Test1",
                   Name = "Test1",
                   Url = "/Test1",
                   SortOrder = 1,
                   ParentId = null,
                   Icon = null
               }
           );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.DeleteFunction("Test1");
          var okResult = result as OkObjectResult;
          Assert.NotNull(okResult);
          var functionVM = okResult.Value as FunctionViewModel;
          Assert.Equal("Test1", functionVM.Id);
      }

        [Fact]
        public async Task DeleteFunction_Validata_Failled()
        {
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.DeleteFunction("Test");
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCommands_HasData_Success()
        {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Function2",
                    Name = "Function2",
                    Url = "/Function2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.AddRange(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                },
                new Command()
                {
                    Id = "Command2",
                    Name = "Command2"
                }
            );
            _context.CommandInFunctions.AddRange(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command2"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function2",
                    CommandId = "Command1"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function2",
                    CommandId = "Command2"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function3",
                    CommandId = "Command2"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.GetCommandsInFunction("Function1");
            var okResult = result as OkObjectResult;
            Assert.NotNull( okResult );
            var commands = okResult.Value as List<CommandViewModel>;
            Assert.True(commands.Count() == 2);
        }
        /*
        [Fact]
        public async Task GetCommandsNotInFunction_HasData_Success()
        {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                },
                new Function()
                {
                    Id = "Function2",
                    Name = "Function2",
                    Url = "/Function2",
                    SortOrder = 2,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.AddRange(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                },
                new Command()
                {
                    Id = "Command2",
                    Name = "Command2"
                }
            );
            _context.CommandInFunctions.AddRange(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command2"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function2",
                    CommandId = "Command1"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function2",
                    CommandId = "Command2"
                },
                new CommandInFunction()
                {
                    FunctionId = "Function3",
                    CommandId = "Command2"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.GetCommandsNotInFunction("Function1");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var commands = okResult.Value as List<CommandViewModel>;
            Assert.True(commands.Count() > 0 );
        }
        
        [Fact]
        public async Task PostCommandToFunction_Validata_Success()
        {
            _context.Functions.AddRange(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
                ,
                new Function()
                {
                    Id = "Function2",
                    Name = "Function2",
                    Url = "/Function2",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.Add(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                }
            );
            _context.CommandInFunctions.Add(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PostCommandToFunction("Function2", new CommandAssignRequest()
            {
                FunctionId = "Function2",
                CommandId = "Command1"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostCommandToFunction_Validata_Fail ()
        {
            _context.Functions.Add(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.Add(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                }
            );
            _context.CommandInFunctions.Add(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.PostCommandToFunction("Function1", new CommandAssignRequest()
            {
                FunctionId = "Function1",
                CommandId = "Command1"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async Task DeleteCommandInFunction_Validata_Success()
        {
            _context.Functions.Add(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.Add(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                }
            );
            _context.CommandInFunctions.Add(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.DeleteCommandInFunction("Function1", "Command1");
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteCommandInFunction_Validata_Fail()
        {
            _context.Functions.Add(
                new Function()
                {
                    Id = "Function1",
                    Name = "Function1",
                    Url = "/Function1",
                    SortOrder = 1,
                    ParentId = null,
                    Icon = null
                }
            );
            _context.Commands.Add(
                new Command()
                {
                    Id = "Command1",
                    Name = "Command1"
                }
            );
            _context.CommandInFunctions.Add(
                new CommandInFunction()
                {
                    FunctionId = "Function1",
                    CommandId = "Command1"
                }
            );
            await _context.SaveChangesAsync();
            var _functionController = new FunctionsController(_context);
            var result = await _functionController.DeleteCommandInFunction("Function1", "Command2");
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        */
    }
}
