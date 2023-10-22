using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KnowledgeSpace.BackendServer.Data;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class UsersControllerTest
    {
        private Mock<UserManager<User>> _mockUserManager { get; set; }
        private Mock<RoleManager<Role>> _mockRoleManager { get; set; }
        private ApplicationDbcontext _context { set; get; }

        private List<User> _resource = new List<User>()
        {
            new User("1","Test1","Test","Test","Test123@gmai.com","0123456789",DateTime.Now),
            new User("2","Test2","Test","Test","Test223@gmai.com","0123456789",DateTime.Now),
            new User("3","Test3","Test","Test","Test323@gmai.com","0123456789",DateTime.Now),
            new User("4","Test4","Test","Test","Test423@gmai.com","0123456789",DateTime.Now)
        };
        private List<Role> _roles = new List<Role>()
        {
            new Role() { Id = "Role1", Name = "Role1"},
            new Role() { Id = "Role2", Name = "Role2" }
        };
        public UsersControllerTest()
        {
            var userStore = new Mock<IUserStore<User>>();
            var roleStore = new Mock<IRoleStore<Role>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _mockRoleManager = new Mock<RoleManager<Role>>(roleStore.Object, null, null, null, null);
            _context = InMemoryDbContextFactory.GetApplicationDbContext("UsersCotrollerTest");
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            Assert.NotNull(userController);
        }
        /*
        [Fact]
        public async Task PostUser_Validate_Success()
        {
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User()
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            var roleController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await roleController.PostUser(new UserCreateRequest()
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Password = "Test@123",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }
        
        [Fact]
        public async Task PostUser_Validate_Failled()
        {
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User()
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PostUser(new UserCreateRequest()
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Password = "Test@123",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        */
        [Fact]
        public async Task GetById_HasData_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.GetById("Test");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var userVM = okResult.Value as UserViewModel;
            Assert.Equal("Test", userVM.UserName);
        }

        [Fact]
        public async Task GetById_ThrowtException_Failled()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Throws<Exception>();
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await userController.GetById("Test"));
        }

        [Fact]
        public async Task GetUsers_HasData_Success()
        {
            _mockUserManager.Setup(x => x.Users).Returns(_resource.AsQueryable().BuildMock());
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = usersController.GetUsers();
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var users = okResult.Value as List<UserViewModel>;
            Assert.True(users.Count() > 0);
            ;
        }

        [Fact]
        public async Task GetUsers_ThrowException_Failled()
        {
            _mockUserManager.Setup(x => x.Users).Throws<Exception>();
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await usersController.GetUsers());
        }

        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            _mockUserManager.Setup(x => x.Users).Returns(_resource.AsQueryable().BuildMock());
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = usersController.GetUsersPaging(1, 2, null);
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var users = okResult.Value as Pagination<UserViewModel>;
            Assert.True(users.TotalRecords == 4 && users.Items.Count() == 2);
        }

        [Fact]
        public async Task GetPaging_NoFilter_Failled()
        {
            _mockUserManager.Setup(x => x.Users).Throws<Exception>();
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await usersController.GetUsersPaging(1, 2, null));
        }

        [Fact]
        public async Task GetPaging_HasFilter_Success()
        {
            _mockUserManager.Setup(x => x.Users).Returns(_resource.AsQueryable().BuildMock());
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = usersController.GetUsersPaging(1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var users = okResult.Value as Pagination<UserViewModel>;
            Assert.True(users.TotalRecords == 1 && users.Items.Count() == 1 && users.Items.Any(x => x.PhoneNumber.Contains("Test1") || x.UserName.Contains("Test1")));
        }

        [Fact]
        public async Task GetPaging_HasFilter_Failled()
        {
            _mockUserManager.Setup(x => x.Users).Throws<Exception>();
            var usersController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            await Assert.ThrowsAnyAsync<Exception>(async () => await usersController.GetUsersPaging(1, 2, "Test1"));
        }
        /*
        [Fact]
        public async Task PutUser_Validata_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUser("Test", new UserCreateRequest()
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Password = "Test@123",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutUser_Validata_Failled()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUser("Test", new UserCreateRequest()
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Password = "Test@123",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async Task DeleteUser_Validata_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.DeleteUser("Test");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var userVM = okResult.Value as UserViewModel;
            Assert.Equal("Test", userVM.Id);
        }
        
        [Fact]
        public async Task DeleteUser_Validata_Failled()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.DeleteUser("Test");
            Assert.IsType<BadRequestObjectResult>(result);
        }
        */
        [Fact]
        public async Task PutUserPassword_Validata_Success()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUserPassword("Test",new UserPasswordChangeRequest()
            {
                UserId = "Test",
                CurrentPassword = "Test@123",
                NewPassword = "Tets@234"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutUserPassword_Validata_Fail()
        {
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User
            {
                Id = "Test",
                UserName = "test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test1234@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0123456789"
            });
            _mockUserManager.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var userController = new UsersController(_mockUserManager.Object, _mockRoleManager.Object, _context);
            var result = await userController.PutUserPassword("Test", new UserPasswordChangeRequest()
            {
                UserId = "Test",
                CurrentPassword = "Test@123",
                NewPassword = "Tets@234"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
