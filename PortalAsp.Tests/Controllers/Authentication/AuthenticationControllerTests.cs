using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PortalAsp.Controllers.Authentication;
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
            mock.Setup(c => c.SignUp(It.IsAny<AuthenticationUserData>())).ReturnsAsync(new GeneralResult {Success = true});
            AuthenticationController controller = new AuthenticationController(mock.Object);

            //Act
            IActionResult postResult = await controller.SignUp(user);
            OkResult okResult = postResult as OkResult;
            BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;

            //Assert
            if (isOk)
            {
                Assert.NotNull(okResult);
                mock.Verify(m => m.SignUp(It.IsAny<AuthenticationUserData>()), Times.Once);
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
                        Login =  "123456",
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

                //Login
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "1234",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Login must be 5-30 characters long. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "1234sadfasdfasdfafasdfasdfasdfasdfasdsdfasafsdfasdfadsfasdfsdafasdfasdfasdfasdfasdf",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Login must be 5-30 characters long. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "1234sadfa sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Login can't contain spaces and quotes(\",')"
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "1234sadfa\"sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Login can't contain spaces and quotes(\",')"
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "1234sadfa'sdfasdfafa",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf12345"
                    },
                    false,
                    "Login can't contain spaces and quotes(\",')"
                };

                //Password
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login = "123456",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf1"
                    },
                    false,
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login = "123456",
                        Email = "myemail@gmail.com",
                        Password = "Avcdf1fadsfasdfhekjqfh23hfkjhsedajkfhsdhajvnbsjkdfdashbvasjhfhewjkfhsdfla"
                    },
                    false,
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login = "123456",
                        Email = "myemail@gmail.com",
                        Password = "avcdf12345"
                    },
                    false,
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login = "123456",
                        Email = "myemail@gmail.com",
                        Password = "Avcdfdafdsfasg"
                    },
                    false,
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. "
                };
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login = "123456",
                        Email = "myemail@gmail.com",
                        Password = "AFDSF12345"
                    },
                    false,
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. "
                };

                //Email
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "123456",
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
                        Login =  "123456",
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
            mock.Setup(c => c.SignUp(It.IsAny<AuthenticationUserData>())).ReturnsAsync(new GeneralResult { Success = true });
            AuthenticationController controller = new AuthenticationController(mock.Object);

            //Act
            IActionResult postResult = await controller.LogIn(user);
            OkResult okResult = postResult as OkResult;
            BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;

            //Assert
            if (isOk)
            {
                Assert.NotNull(okResult);
                mock.Verify(m => m.LogIn(It.IsAny<AuthenticationUserData>()), Times.Once);
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
                        Login =  "123456",
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

                //Login
                yield return new object[]
                {
                    new AuthenticationUserData
                    {
                        Login =  "",
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
                        Login =  "123441241",
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
