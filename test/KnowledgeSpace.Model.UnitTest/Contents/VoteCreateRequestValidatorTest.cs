using KnowledgeSpace.Model.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Contents
{
    public class VoteCreateRequestValidatorTest
    {
        private VoteCreateRequest _request { get; set; }
        private VoteCreateRequestValidator _validate { get; set; }
        public VoteCreateRequestValidatorTest()
        {
            _validate = new VoteCreateRequestValidator();
            _request = new VoteCreateRequest()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            };
        }

        [Fact]
        public void Should_Validator_Success()
        {
            var result = _validate.Validate(_request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Fail_When_KnowledgeBaseId_Error()
        {
            _request.KnowledgeBaseId = 0;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_UserId_Error(string userId)
        {
            _request.UserId = userId;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
