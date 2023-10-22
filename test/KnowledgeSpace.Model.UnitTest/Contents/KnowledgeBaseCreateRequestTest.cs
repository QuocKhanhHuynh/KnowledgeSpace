using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Contents
{/*
    public class KnowledgeBaseCreateRequestTest
    {
        private KnowledgeBaseCreateRequest _request { get; set; }
        private KnowledgeBaseCreateRequestValidator _validate { get; set; }
        public KnowledgeBaseCreateRequestTest()
        {
            _validate = new KnowledgeBaseCreateRequestValidator();
            _request = new KnowledgeBaseCreateRequest()
            {
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage= "Test",
                //Labels = "Test" ,
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias= "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
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
        public void Fail_When_Title_Error(string title)
        {
            _request.Title = title;
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

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_NewPassword_Error(string ownerUserId)
        {
            _request.OwnerUserId = ownerUserId;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }*/
}
