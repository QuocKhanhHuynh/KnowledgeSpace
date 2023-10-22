using KnowledgeSpace.Model.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Contents
{
    public class CommentCreateRequestValidatorTest
    {
        private CommentCreateRequest _request { get; set; }
        private CommentCreateRequestValidator _validate { get; set; }
        public CommentCreateRequestValidatorTest()
        {
            _validate = new CommentCreateRequestValidator();
            _request = new CommentCreateRequest()
            {
                Content = "Comment",
                KnowledgeBaseId = 1
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
        public void Fail_When_Content_Error(string content)
        {
            _request.Content = content;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Fail_When_KnowledgeBaseId_Error()
        {
            _request.KnowledgeBaseId = 0;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
