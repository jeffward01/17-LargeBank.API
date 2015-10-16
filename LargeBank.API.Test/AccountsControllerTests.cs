using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LargeBank.API.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;
using LargeBank.API.Models;

namespace LargeBank.API.Test
{
    [TestClass]
    public class AccountsControllerTests
    {
       [TestMethod]
        public void GetTransactionsReturnTransactions()
        {
            //Arrange
            var accountsController = new AccountsController();

            //Act
            IHttpActionResult result = accountsController.GetTransactions(1);

            //Assert
            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            //If action returns: Ok()
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IQueryable<TransactionModel>>));

        }

        [TestMethod]
        public void GetAccountsReturnAccounts()
        {
            //Arrange
            var accountsController = new AccountsController();

            //Act
            IQueryable<AccountModel> accounts = accountsController.GetAccounts();

            //Assert
            Assert.IsTrue(accounts.Count() > 0);

        }

        [TestMethod]
        public void GetAccountsForCustomerReturnAccounts()
        {
            //Arrange
            var AccountsController = new AccountsController();

            //Act
            IHttpActionResult result = AccountsController.GetAccounts(1);

            //Assert
            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            //If action returns: Ok()
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IQueryable<AccountModel>>));

        }

        [TestMethod]
        public void GetAccountbyIDReturnAccount()
        {
            //Arrange
            var accountsController = new AccountsController();

            //Act
            IHttpActionResult result = accountsController.GetAccount(1);

            //Assert
            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            //If action returns: Ok()
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountModel>));

        }

        //Broken for some reason.  Bad test.
        [TestMethod]
        public void PutAccountUpdateAccount()
        {
            IHttpActionResult result;
            CreatedAtRouteNegotiatedContentResult<AccountModel> contentResult;
            OkNegotiatedContentResult<AccountModel> accountResult;

            //Arrange
            using (var accountsController = new AccountsController())
            {
                //Build new AccountModel Object
                var newAccount = new AccountModel
                {
                    AccountNumber = 21323,
                    Balance = 213213,


                };
                //Insert AccountModelObject into Database so 
                //that I can take it out and test for update.
                 result = accountsController.PostAccount(newAccount);

                //Cast result as Content Result so that I can gather information from ContentResult
                 contentResult = (CreatedAtRouteNegotiatedContentResult<AccountModel>)result;
            }
            using (var SecondaccountsController = new AccountsController())
            {
                //Result contains the customer I had JUST createad
                result = SecondaccountsController.GetAccount(contentResult.Content.AccountId);

                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountModel>));

                //Get AccountModel from 'result'
                accountResult = (OkNegotiatedContentResult<AccountModel>)result;
            }

            using (var thirdController = new AccountsController())
            { 
                var modifiedAccount = accountResult.Content;

                modifiedAccount.Balance += 5;

                //Act
                //The result of the Put Request
                result = thirdController.PutAccount(accountResult.Content.AccountId, modifiedAccount);

                //Assert
                Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            }
        }

        [TestMethod]
        public void DeleteAccountRecord()
        {
            //Arrange
            //Create Controller
            var customersController = new AccountsController();

            //Create a customer to be deleted
            var dbAccount = new AccountModel
            {
                AccountNumber = 21323,
                Balance = 213213,

            };

            //Add 'new customer' to the DB using a POST
            //Save returned value as RESULT
            IHttpActionResult result = customersController.PostAccount(dbAccount);

            //Cast result as Content Result so that I can gather information from ContentResult
            CreatedAtRouteNegotiatedContentResult<AccountModel> contentResult = (CreatedAtRouteNegotiatedContentResult<AccountModel>)result;


            //Result contains the customer I had JUST created
            result = customersController.GetAccount(contentResult.Content.AccountId);

            //Get CustomerModel from 'result'
            OkNegotiatedContentResult<AccountModel> customerResult = (OkNegotiatedContentResult<AccountModel>)result;


            //Act
            //The result of the Delete Request
            result = customersController.DeleteAccount(customerResult.Content.AccountId);

            //Assert

            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountModel>));
        }

    }
}
