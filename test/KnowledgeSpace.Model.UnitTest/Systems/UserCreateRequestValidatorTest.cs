using KnowledgeSpace.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.Model.UnitTest.Systems
{
    public class UserCreateRequestValidatorTest
    {
        private UserCreateRequest _request;
        private UserCreateRequestValidator _validator;
        public UserCreateRequestValidatorTest()
        {
            _request = new UserCreateRequest()
            {
                UserName = "Test",
                FirstName= "Test",
                LastName= "Test",
                Dob = "01/01/2002",
                Email = "Test123@gmail.com",
                Password = "Test@123",
                PhoneNumber = "1234567890",
            };
            _validator = new UserCreateRequestValidator();
        }
        [Fact]
        public void Should_Validator_Success()
        {
            var result = _validator.Validate(_request);
            Assert.True(result.IsValid);
        }
        /*
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_UserName(string userName)
        {
            _request.UserName = userName;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }
        */
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_FirstName(string firstName)
        {
            _request.FirstName = firstName;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_LastName(string lastName)
        {
            _request.LastName = lastName;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_PhoneNumber(string phoneNumber)
        {
            _request.PhoneNumber = phoneNumber;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Faill_When_Error_Email(string email)
        {
            _request.Email = email;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }

        [Theory]
        //[InlineData("")]
       // [InlineData(null)]
        [InlineData("test@123")]
        [InlineData("Test1234")]
        [InlineData("Test@12")]
        public void Faill_When_Error_Password(string password)
        {
            _request.Password = password;
            var result = _validator.Validate(_request);
            Assert.False(result.IsValid);
        }
    }
}
