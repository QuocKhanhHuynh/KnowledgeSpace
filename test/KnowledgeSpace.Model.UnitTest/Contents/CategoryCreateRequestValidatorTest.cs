using KnowledgeSpace.Model.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Contents
{
    public class CategoryCreateRequestValidatorTest
    {
        private CategoryCreateRequest _request { get; set; }
        private CategoryCreateRequestValidator _validate { get; set; }
        public CategoryCreateRequestValidatorTest()
        {
            _validate = new CategoryCreateRequestValidator();
            _request = new CategoryCreateRequest()
            {
                Name = "Category",
                ParentId = 1,
                SeoAlias = "/Category",
                SeoDescription = "Category",
                SortOrder = 1
            };
        }

        [Fact]
        public void Should_Validator_Success()
        {
            var result = _validate.Validate(_request);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_Name_Error(string name)
        {
            _request.Name = name;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_SeoAlias_Error(string seoAlias)
        {
            _request.SeoAlias = seoAlias;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Fail_When_SortOrder_Error()
        {
            _request.SortOrder = 0;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
