using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.UI;
using CROPS.HttpClientUtility;
using CROPS.Users.Contracts;
using CROPS.Users.DTOs;
using Moq;
using Shouldly;
using Xunit;

namespace CROPS.Tests.Users
{
    [Trait("Category", "UesrManagement")]
    public class UsersTests : CROPSTestBase
    {
        private const string RoleNotExistMsg = "The role of the user does not exist";
        private const string EmailNotExistnMsg = "This email is not exist";
        private const string EmailNotValidMsg = "This email is not valid";
        private const string EmailAlreadyExistMsg = "This email is already exist";
        private const string UserNameAlreadyExistMsg = "This name is already exist";
        private const string NoClaimsSentMsg = "There are no claims sent";
        private const string UserIdNotExist = "This id is not exist";
        private const string UserNotExistOrNotActive = "There is no user with this id or the user is not active";
        private const string UserNotExist = "This user is not exist";

        private Mock<IHttpClientManager> mockHttpClient = new Mock<IHttpClientManager>();

        private readonly IUsersAppService _usersManagementAppService;

        public UsersTests()
        {
            RegisterInstance<IHttpClientManager>(mockHttpClient.Object);

            _usersManagementAppService = Resolve<IUsersAppService>();
        }

        #region Get

        [Fact]
        public async Task Get_ValidUser_ShouldReturnUser()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"userName\": \"Jack.John\",\"email\": \"Jack.John@domain.com\"}") }));

            // Act
            var createdUser = await _usersManagementAppService.Get(user);

            // Assert
            createdUser.UserName.ShouldBe(user.UserName);
            createdUser.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task Get_UserWithInvalidId_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John55@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(UserNotExistOrNotActive) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Get(user).ConfigureAwait(false)).Message.ShouldBe(UserNotExistOrNotActive);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_ValidUser_ShouldBeCreated()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"userName\": \"Jack.John\",\"email\": \"Jack.John@domain.com\"}") }));

            // Act
            var createdUser = await _usersManagementAppService.Create(user);

            // Assert
            createdUser.UserName.ShouldBe(user.UserName);
            createdUser.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task Create_UserWithExistingName_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John55@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(UserNameAlreadyExistMsg) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Create(user).ConfigureAwait(false)).Message.ShouldBe(UserNameAlreadyExistMsg);
        }

        [Fact]
        public async Task Create_UserWithExistingMail_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(EmailAlreadyExistMsg) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Create(user).ConfigureAwait(false)).Message.ShouldBe(EmailAlreadyExistMsg);
        }

        [Fact]
        public async Task Create_UserWithInvalidRole_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(RoleNotExistMsg) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Create(user).ConfigureAwait(false)).Message.ShouldBe(RoleNotExistMsg);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_ValidUser_ShouldReturnOk()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PutAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"userName\": \"Jack.John\",\"email\": \"Jack.John@domain.com\"}") }));

            // Assert
            var createdUser = await _usersManagementAppService.Update(user);

            createdUser.UserName.ShouldBe(user.UserName);
            createdUser.Email.ShouldBe(user.Email);
        }

        [Fact]
        public async Task Update_NotExistingUser_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PutAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(UserNotExist) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Update(user).ConfigureAwait(false)).Message.ShouldBe(UserNotExist);
        }

        [Fact]
        public async Task Update_UserWithInvalidRole_ShouldThrowException()
        {
            UserRoleDTO user = new UserRoleDTO()
            {
                UserName = "Jack.John",
                Email = "Jack.John@domain.com",
                Password = "P@$$w0rd",
                RoleName = "Manager"
            };

            mockHttpClient
                .Setup(e => e.PutAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(RoleNotExistMsg) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Update(user).ConfigureAwait(false)).Message.ShouldBe(RoleNotExistMsg);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_UserWithInvalidId_ShouldThrowException()
        {
            mockHttpClient
                .Setup(e => e.DeleteAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(UserIdNotExist) }));

            // Assert
            Should.Throw<UserFriendlyException>(async () => await _usersManagementAppService.Delete(new EntityDto<Guid>() { Id = Guid.Empty }).ConfigureAwait(false)).Message.ShouldBe(UserIdNotExist);
        }

        #endregion
    }
}
