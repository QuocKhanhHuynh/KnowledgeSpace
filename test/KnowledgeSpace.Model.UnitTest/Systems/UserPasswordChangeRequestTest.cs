using KnowledgeSpace.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Systems
{
    public class UserPasswordChangeRequestTest
    {
        private UserPasswordChangeRequest _request { get; set; }
        private UserPasswordChangeRequestValidator _validate { get; set; }
        public UserPasswordChangeRequestTest()
        {
            _validate = new UserPasswordChangeRequestValidator();
            _request = new UserPasswordChangeRequest()
            {
                UserId = "Test",
                CurrentPassword = "Test@123",
                NewPassword = "Test@012"
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
        public void Fail_When_UserId_Error(string userId)
        {
            _request.UserId = userId;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_CurrentPassword_Error(string currentPassword)
        {
            _request.CurrentPassword = currentPassword;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_When_NewPassword_Error(string newPassword)
        {
            _request.NewPassword = newPassword;
            var result = _validate.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
