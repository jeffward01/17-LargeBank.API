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
    public class TransactionsControllerTests
    {

        [TestMethod] //{1}
        public void GetTransactionsReturnTransactions()
        {
            //Arrange
            var transactionsController = new TransactionsController();

            //Act
            IQueryable<TransactionModel> results = transactionsController.GetTransactions();

            //Assert (if true, pass)
            Assert.IsTrue(results.Count() > 0);
        }

        [TestMethod] //{2}
        public void GetTransactionReturnTransaction()
        {
            //Arrange
            var transactionController = new TransactionsController();

            //Act
          
            IHttpActionResult result = transactionController.GetTransaction(1);

            //Assert
            //If not found
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));


            //If is found, and returned
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TransactionModel>));
            
        }

        [TestMethod]  //{3}
        public void GetAllTransactionsForAccountIdReturnTransactions()
        {
            //arrange
            var transactionsController = new TransactionsController();

            //act
            IHttpActionResult result = transactionsController.GetTransactions(1);

            //assert
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IQueryable<TransactionModel>>));
        }

        [TestMethod]  //{4}
        public void PutTransactionUpdateTransaction()
        {
            IHttpActionResult result;
            CreatedAtRouteNegotiatedContentResult<TransactionModel> contentResult;
            OkNegotiatedContentResult<TransactionModel> transactionResult;


            //Arrange
            using (var transactionsController = new TransactionsController())
            {

                var tsModel = new TransactionModel
                {
                    Amount = 123,                
                };

                result = transactionsController.PostTransaction(tsModel);

                //Cast result as Content Result so that I can gather information from ContentResult
                contentResult = (CreatedAtRouteNegotiatedContentResult<TransactionModel>)result;
            }

            using (var secondTransactionsController = new TransactionsController())
            {

                //Result contains the customer I had JUST createad
                result = secondTransactionsController.GetTransaction(1);

                Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TransactionModel>));
                
                //Get transactionModel from 'result'
                transactionResult = (OkNegotiatedContentResult<TransactionModel>)result;
           
             }

            using (var thirdTransactionsController = new TransactionsController())
            {
                var modifiedContent = transactionResult.Content;

                modifiedContent.Amount += 5;

                //Act
                result = thirdTransactionsController.PutTransaction(transactionResult.Content.TransactionId, modifiedContent);
                //Assert
                Assert.IsInstanceOfType(result, typeof(StatusCodeResult));

            }
        }

        [TestMethod] // {5}
        public void PostTransactionCreateTransactions()
        {
            //Arrange
            var transactionsController = new TransactionsController();

            //Act
            var newTransaction = new TransactionModel
            {
                Amount = 12,

            };

            IHttpActionResult result = transactionsController.PostTransaction(newTransaction);

            //Assert

            //If not 'true' Assert False
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<TransactionModel>));

            //Cast
            CreatedAtRouteNegotiatedContentResult<TransactionModel> contentResult = (CreatedAtRouteNegotiatedContentResult<TransactionModel>)result;

            //Check if Customer is posted to the database
            //Check to see if Customer ID is NOT equal to zero.  If Customer Id us equal to zero,
            //then customer was NOT added to Database
            Assert.IsTrue(contentResult.Content.AccountId!= 0);
        }

        [TestMethod] //{6}
        public void DeleteTransactionDeleteTransaction()
        {
            //Arrange
            //Create Controller
            var transactionsController = new TransactionsController();
            
            //Create a customer to be deleted
            var dbTransactions = new TransactionModel
            {
                Amount = 21323,
               
            };

            //Add 'new customer' to the DB using a POST
            //Save returned value as RESULT
            IHttpActionResult result = transactionsController.PostTransaction(dbTransactions);

            //Cast result as Content Result so that I can gather information from ContentResult
            CreatedAtRouteNegotiatedContentResult<TransactionModel> contentResult = (CreatedAtRouteNegotiatedContentResult<TransactionModel>)result;


            //Result contains the customer I had JUST created
            result = transactionsController.GetTransaction(1);

            //Get CustomerModel from 'result'
            OkNegotiatedContentResult<TransactionModel> customerResult = (OkNegotiatedContentResult<TransactionModel>)result;
            

            //Act
            //The result of the Delete Request
           IHttpActionResult second = transactionsController.DeleteTransaction(1);

            //Assert

            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(second, typeof(NotFoundResult));

            Assert.IsInstanceOfType(second, typeof(OkNegotiatedContentResult<TransactionModel>));
        }

    }


}
