using Auth.Controllers;
using Auth.Models.AuthViewModels;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace Auth.Tests.Controller
{
    public class AuthControllerTest
    {
        /// <summary>
        /// This method tests the user registration.
        /// </summary>
        [Fact]
        public void UserRegistrationTest()
        {
            var model = new RegisterViewModel
            {
                Email = "new@app.de",
                Password = "abc",
                ConfirmPassword = "abc"
            };

            MyMvc
                .Controller<AuthController>()
                .Calling(c => c.Register(model))
                .ShouldReturn()
                .BadRequest();
        }
    }
}
