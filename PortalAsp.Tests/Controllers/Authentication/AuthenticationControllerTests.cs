using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PortalAsp.Controllers.Auth;
using PortalModels;
using PortalModels.Authentication;
using Xunit;

namespace PortalAsp.Tests.Controllers.Authentication
{
    public class AuthenticationControllerTests
    {
        [Theory]
        [ClassData(typeof(BadRequestSignUpData))]
        public async Task SignUp_DropsBadRequestOnInvalidUserData(AuthenticationUserData user, bool isOk, string errorMessage = "")
        {
            //Arrange
            Mock<IUserAuthenticator> mock = new Mock<IUserAuthenticator>();
            mock.Setup(c => c.SignUpAsync(It.IsAny<AuthenticationUserData>())).ReturnsAsync(new GeneralResult {Success = true});
            AuthController controller = new AuthController(mock.Object);

            //Act
            IActionResult postResult = await controller.SignUp(user);
            OkResult okResult = postResult as OkResult;
            BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;

            //Assert
            if (isOk)
            {
                Assert.NotNull(okResult);
                mock.Verify(m => m.SignUpAsync(It.IsAny<AuthenticationUserData>()), Times.Once);
            }
            else
            {
                Assert.NotNull(badRequestResult);
                Assert.Equal(badRequestResult.Value.ToString(), errorMessage);
            }
        }

        public class BadRequestSignUpData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "123456",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    true
                };

                //null
                yield return new object[]
                {
                    null,
                    false,
                    "User is null"
                };

                //UserName
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "1234",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "UserName must be 5-30 characters long. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "1234sadfasdfasdfafasdfasdfasdfasdfasdsdfasafsdfasdfadsfasdfsdafasdfasdfasdfasdfasdf",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "UserName must be 5-30 characters long. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "1234sadfa sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "UserName can't contain spaces and quotes(\",')"
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "1234sadfa\"sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "UserName can't contain spaces and quotes(\",')"
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "1234sadfa'sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "UserName can't contain spaces and quotes(\",')"
                };

                //Email
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "12345",
                        Email = "myemai.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Email is invalid"
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "12345",
                        Email = "myemai",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Email is invalid"
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Theory]
        [ClassData(typeof(BadRequestLogInData))]
        public async Task LogIn_DropsBadRequestOnInvalidUserData(AuthenticationUserData user, bool isOk, string errorMessage = "")
        {
            //Arrange
            Mock<IUserAuthenticator> mock = new Mock<IUserAuthenticator>();
            mock.Setup(c => c.LogInOrReturnNullAsync(It.IsAny<AuthenticationUserData>())).ReturnsAsync(new LoginSuccessfulData());
            AuthController controller = new AuthController(mock.Object);

            //Act
            IActionResult postResult = await controller.LogIn(user);
            OkObjectResult okResult = postResult as OkObjectResult;
            BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;
            
            //Assert
            if (isOk)
            {
                Assert.NotNull(okResult);
                mock.Verify(m => m.LogInOrReturnNullAsync(It.IsAny<AuthenticationUserData>()), Times.Once);
            }
            else
            {
                Assert.NotNull(badRequestResult);
                Assert.Equal(badRequestResult.Value.ToString(), errorMessage);
            }
        }

        public class BadRequestLogInData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "12345",
                        Password = "Avcdf12345"
                    },
                    true
                };

                //null
                yield return new object[]
                {
                    null,
                    false,
                    "User is null"
                };

                //UserName
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Wrong login or password. "
                };

                //Password
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        UserName =  "123441241",
                        Password = ""
                    },
                    false,
                    "Wrong login or password. "
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
