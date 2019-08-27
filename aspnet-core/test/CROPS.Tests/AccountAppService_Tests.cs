using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using AutoMapper;
using CROPS.Accounts;
using CROPS.Accounts.Contracts;
using CROPS.Accounts.DTOs;
using CROPS.Projects;
using Moq;
using Shouldly;
using Xunit;

namespace CROPS.Tests
{
    public class AccountAppService_Tests : CROPSTestBase
    {
        private readonly IAccountAppService accoutService;
        Mock<IRepository<Account>> repo;
        List<Account> accounts = new List<Account>();
        public AccountAppService_Tests()
        {
            repo = new Mock<IRepository<Account>>();

            repo.Setup(x => x.InsertAsync(It.IsAny<Account>()))
                .Callback((Account account) => accounts.Add(account));

            repo.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns((Expression<Func<Account, bool>> expression) => { return accounts.FirstOrDefault(expression.Compile()); });

            accoutService = Resolve<IAccountAppService>();
        }


        [Fact]
        public void CreateAccount_NewUser_Success()
        {
            // Arrange
            var input = new AccountCreateDto
            {
                AccountName = "Account",
                Descriptions = "Description"
            };


            // Act
            var result = this.accoutService.Create(input).Result;

            // Assert
            result.ShouldBeOfType<AccountDto>().ShouldNotBeNull();
            result.AccountName.ShouldBe<string>("Account");
        }

        [Fact]
        public async Task CreateAccount_DuplicateUser_UserFriendlyException()
        {
            // Arrange
            var input = new AccountCreateDto
            {
                AccountName = "Account",
                Descriptions = "Description"
            };


            // Act
            this.accoutService.Create(input);

            // Assert
            var ex = Assert.ThrowsAsync<UserFriendlyException>(async () => await this.accoutService.Create(input));
            Assert.True(ex.Result.Message == "Account already exists.");
        }

        [Fact]
        public async Task CreateAccount_Empty_Description_UserFriendlyException()
        {
            // Arrange
            var input = new AccountCreateDto
            {
                AccountName = "Account"
            };

                
            var ex = Assert.Throws<AbpValidationException>(() => this.accoutService.Create(input).Result);
            ex.ValidationErrors.Count().ShouldBe<int>(1);
        }

        [Fact]
        public void CreateAccount_Empty_Name_UserFriendlyException()
        {
            // Arrange
            var input = new AccountCreateDto
            {
                Descriptions = "Descriptions"
            };

            // Assert
            var ex = Assert.Throws<AbpValidationException>(() => this.accoutService.Create(input).Result);
            ex.ValidationErrors.Count().ShouldBe<int>(1);
        }
    }
}
