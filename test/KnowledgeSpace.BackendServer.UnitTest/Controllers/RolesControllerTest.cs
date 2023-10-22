using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KnowledgeSpace.Model;
using static System.Net.Mime.MediaTypeNames;
using KnowledgeSpace.BackendServer.Data;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class RolesControllerTest
    {
        private Mock<RoleManager<Role>> _mockRoleManager { set; get; }
        private ApplicationDbcontext _context { set; get; }
        private List<Role> _resource = new List<Role>()
        {
            //new Role(){ Id = "Test1", Name = "Test1"},
            new Role(){ Id = "Test2", Name = "Test2"},
            new Role(){ Id = "Test3", Name = "Test3"},
            new Role(){ Id = "Test4", Name = "Test4"}
        };
        public RolesControllerTest()
        {
            var roleStore = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(roleStore.Object, null, null, null, null);
            _context = InMemoryDbContextFactory.GetApplicationDbContext("RoleConreollersTest");
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            Assert.NotNull(roleController);
        }

        [Fact]
        public async Task PostRole_Validate_Success()
        {
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.PostRole(new RoleCreateRequest()
            {
                Id = "Test",
                Name = "Test"
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostRole_Validate_Failled()
        {
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] {}));
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.PostRole(new RoleCreateRequest()
            {
                Id = "Test",
                Name = "Test"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new Role
            {
                Id = "Test",
                Name = "Test"
            });
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.GetById("Test");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var roleVM = okResult.Value as RoleViewModel;
            Assert.Equal("Test", roleVM.Name);
        }

        [Fact]
        public async Task GetById_ThrowtException_Failled()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Throws<Exception>();
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await roleController.GetById("Test"));
        }

        [Fact]
        public async Task GetRoles_HasData_Success()
        {
            _mockRoleManager.Setup(x => x.Roles).Returns(_resource.AsQueryable().BuildMock());
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            var result = rolesController.GetRoles();
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var roles = okResult.Value as List<RoleViewModel>;
            Assert.True(roles.Count() > 0);
;        }

        [Fact]
        public async Task GetRoles_ThrowException_Failled()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await rolesController.GetRoles());
        }
        /*
        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            _mockRoleManager.Setup(x => x.Roles).Returns(_resource.AsQueryable().BuildMock());
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            var result = rolesController.GetRolesPaging(1,2,null);
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var roles = okResult.Value as Pagination<RoleViewModel>;
            Assert.True(roles.TotalRecords == 4 && roles.Items.Count() == 2);
        }
        */
        [Fact]
        public async Task GetPaging_NoFilter_Failled ()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await rolesController.GetRolesPaging(1, 2, null));
        }
        /*
        [Fact]
        public async Task GetPaging_HasFilter_Success()
        {
            _mockRoleManager.Setup(x => x.Roles).Returns(_resource.AsQueryable().BuildMock());
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            var result = rolesController.GetRolesPaging(1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var roles = okResult.Value as Pagination<RoleViewModel>;
            Assert.True(roles.TotalRecords == 1 && roles.Items.Count() == 1 && roles.Items.Any(x => x.Id.Equals("Test1") || x.Name.Equals("Test1")));
        }
        */
        [Fact]
        public async Task GetPaging_HasFilter_Failled()
        {
            _mockRoleManager.Setup(x => x.Roles).Throws<Exception>();
            var rolesController = new RolesController(_mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await rolesController.GetRolesPaging(1, 2, "Test1"));
        }

        [Fact]
        public async Task PutRole_Validata_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new Role
            {
                Id = "Test",
                Name = "Test"
            });
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.PutRole("Test", new RoleCreateRequest()
            {
                Id = "Test",
                Name = "Test"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutRole_Validata_Failled()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new Role
            {
                Id = "Test",
                Name = "Test"
            });
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] {}));
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.PutRole("Test", new RoleCreateRequest()
            {
                Id = "Test",
                Name = "Test"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRole_Validata_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new Role
            {
                Id = "Test",
                Name = "Test"
            });
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.DeleteRole("Test");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var roleVM = okResult.Value as RoleViewModel;
            Assert.Equal("Test", roleVM.Id);
        }

        [Fact]
        public async Task DeleteRole_Validata_Failled()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new Role
            {
                Id = "Test",
                Name = "Test"
            });
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<Role>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] {}));
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.DeleteRole("Test");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetPermissionByRoleId_HasData_Success()
        {
            _context.Roles.AddRange(
                new Role()
                {
                    Id = "Role1",
                    Name = "Role1"
                },
                 new Role()
                 {
                     Id = "Role2",
                     Name = "Role2"
                 }
            );
            _context.Functions.Add(new Function() { Id = "Fucntion", Name = "Fucntion", Url = "/Fucntion", SortOrder = 0, ParentId = null, Icon = null });
            _context.Commands.AddRange(new Command() { Id = "Command1", Name = "Command1" }, new Command() { Id = "Command2", Name = "Command2" });
            _context.Permissions.AddRange(new Permission("Function","Role1","Command1"), new Permission("Function", "Role2", "Command1"), new Permission("Function", "Role2", "Command2"));
            await _context.SaveChangesAsync();
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var result = await roleController.GetPermissionByRoleId("Role2");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var permissions = okResult.Value as List<PermissionViewModel>;
            Assert.True(permissions.Count() == 2);
        }

        [Fact]
        public async Task PutPermissionByRoleId_HasData_Success()
        {
            _context.Roles.Add( new Role(){ Id = "Role", Name = "Role" });
            _context.Functions.Add(new Function() { Id = "Fucntion", Name = "Fucntion", Url = "/Fucntion", SortOrder = 0, ParentId = null, Icon = null });
            _context.Commands.Add(new Command() { Id = "Command", Name = "Command" });
            _context.Permissions.Add(new Permission("Function", "Role", "Command"));
            await _context.SaveChangesAsync();
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var request = new UpdatePermissionRequest();
            request.Permissions.Add(new PermissionViewModel() { RoleId = "Role", CommandId = "Command1", FunctionId = "Function1" });
            var result = await roleController.PutPermissionsById("Role", request);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutPermissionByRoleId_HasData_Fail()
        {
            _context.Roles.Add(new Role() { Id = "Role", Name = "Role" });
            _context.Functions.Add(new Function() { Id = "Fucntion", Name = "Fucntion", Url = "/Fucntion", SortOrder = 0, ParentId = null, Icon = null });
            _context.Commands.Add(new Command() { Id = "Command", Name = "Command" });
            _context.Permissions.Add(new Permission("Function", "Role", "Command"));
            await _context.SaveChangesAsync();
            var roleController = new RolesController(_mockRoleManager.Object, _context);
            var request = new UpdatePermissionRequest();
            request.Permissions.Add(new PermissionViewModel() { RoleId = "Role1", CommandId = "Command1", FunctionId = "Function1" });
            var result = await roleController.PutPermissionsById("Role1", request);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        
    }
}
