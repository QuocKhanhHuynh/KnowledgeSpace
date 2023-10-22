using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class PermissionsControllerTest
    {
        private Mock<IConfigurationRoot> _mockConfiguration;
        public PermissionsControllerTest()
        {
            _mockConfiguration = new Mock<IConfigurationRoot>();
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var _voteController = new PermissionsController(_mockConfiguration.Object);
            Assert.NotNull(_voteController);
        }
        
        [Fact]
        public async Task GetPermissions_HasData_Success()
        {
            
            _mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("the string you want to return");
            var controller = new PermissionsController(_mockConfiguration.Object);
            Assert.NotNull(controller);
        }
    }
}
