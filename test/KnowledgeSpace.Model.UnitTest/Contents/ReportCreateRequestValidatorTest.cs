using KnowledgeSpace.Model.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Contents
{
    public class ReportCreateRequestValidatorTest
    {
        private ReportCreateRequest _request { get; set; }
        private ReportCreateRequestValidator _validate { get; set; }
        public ReportCreateRequestValidatorTest()
        {
            _validate = new ReportCreateRequestValidator();
            _request = new ReportCreateRequest()
            {
                KnowledgeBaseId = 1,
                ReportUserId = "User",
                Content = "Test"
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
        public void Fail_When_ReportuserId_Error(string reportUserId)
        {
            _request.ReportUserId = reportUserId;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
