using KnowledgeSpace.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Systems
{
    public class RoleCreateRequestValidatorTest
    {
        private RoleCreateRequest _request;
        private RoleCreateRequestValidator _validator;
        public RoleCreateRequestValidatorTest()
        {
            _request = new RoleCreateRequest()
            {
                Id = "Admin",
                Name = "Admin"
            };
            _validator = new RoleCreateRequestValidator();
        }
        [Fact]
        public void Should_Validator_Success()
        {
            var result = _validator.Validate(_request);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_Error_Id(string id)
        {
            _request.Id = id;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_Error_Name(string name)
        {
            _request.Name = name;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
