using KnowledgeSpace.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Systems
{
    public class FunctionCreateRequestValidatorTest
    {
        private FunctionCreateRequest _request;
        private FunctionCreateRequestValidator _validator;
        public FunctionCreateRequestValidatorTest()
        {
            _request = new FunctionCreateRequest()
            {
                Id = "Test",
                Name = "Test",
                ParentId = null,
                SortOrder = 1,
                Url= "/Test",
                Icon = null
            };
            _validator = new FunctionCreateRequestValidator();
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
        public void Faill_When_Error_Id(string id)
        {
            _request.Id = id;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_Name(string name)
        {
            _request.Name = name;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_Url(string url)
        {
            _request.Url = url;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
