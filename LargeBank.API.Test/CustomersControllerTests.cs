using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LargeBank.API.Controllers;
using System.Collections.Generic;
using LargeBank.API.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace LargeBank.API.Test
{
    [TestClass]
    public class CustomerControllerTests
    {
        [TestMethod]
        public void GetCustomersReturnsCustomers()
        {
            //Arrange
            //Properties go here
            var customerController = new CustomersController();

            //Act
            //Call the method in question that needs to be tested
            IEnumerable<CustomerModel> customers = customerController.GetCustomers();


            //Assert
            //You assert that if the outcome is this, assert that.
            Assert.IsTrue(customers.Count() > 0);
        }

        [TestMethod]
        public void PostCustomerCreatesCustomer()
        {
            //Arrange
            var customerController = new CustomersController();

            //Act
            var newCustomer = new CustomerModel
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Address1 = "123 Main Street",
                Address2 = "Suite 2",
                City = "San Diego",
                States = "CA",
                Zip = "92101"
            };

            //The result of the Post Request
            IHttpActionResult result = customerController.PostCustomer(newCustomer);

            //Assert

            //If not 'true' Assert False
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<CustomerModel>));

            //Cast
            CreatedAtRouteNegotiatedContentResult<CustomerModel> contentResult = (CreatedAtRouteNegotiatedContentResult<CustomerModel>)result;

            //Check if Customer is posted to the database
            //Check to see if Customer ID is NOT equal to zero.  If Customer Id us equal to zero,
            //then customer was NOT added to Database
            Assert.IsTrue(contentResult.Content.CustomerId != 0);
        }

        [TestMethod]
        public void PutCustomerUpdatesCustomer()
        {
            //Arrange
            var cusomersContoller = new CustomersController();

            var newCustomer = new CustomerModel
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Address1 = "123 Main Street",
                Address2 = "Suite 2",
                City = "San Diego",
                States = "CA",
                Zip = "92101"
            };

            //The result of the Post Request
            IHttpActionResult result = cusomersContoller.PostCustomer(newCustomer);

            //Cast result as Content Result so that I can gather information from ContentResult
            CreatedAtRouteNegotiatedContentResult<CustomerModel> contentResult = (CreatedAtRouteNegotiatedContentResult<CustomerModel>)result;


            //Result contains the customer I had JUST createad
            result = cusomersContoller.GetCustomer(contentResult.Content.CustomerId);

            //Get CustomerModel from 'result'
            OkNegotiatedContentResult<CustomerModel> customerResult = (OkNegotiatedContentResult<CustomerModel>)result;


            //Act
            //The result of the Put Request
            result = cusomersContoller.PutCustomer(customerResult.Content.CustomerId, newCustomer);

            //Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));

        }

        [TestMethod]
        public void DeleteCustomerRecord()
        {
            //Arrange
            //Create Controller
            var customersController = new CustomersController();

            //Create a customer to be deleted
            var dbCustomer = new CustomerModel
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Address1 = "123 Main Street",
                Address2 = "Suite 2",
                City = "San Diego",
                States = "CA",
                Zip = "92101",
           
            };

            //Add 'new customer' to the DB using a POST
            //Save returned value as RESULT
            IHttpActionResult result = customersController.PostCustomer(dbCustomer);

            //Cast result as Content Result so that I can gather information from ContentResult
            CreatedAtRouteNegotiatedContentResult<CustomerModel> contentResult = (CreatedAtRouteNegotiatedContentResult<CustomerModel>)result;


            //Result contains the customer I had JUST createad
            result = customersController.GetCustomer(contentResult.Content.CustomerId);

            //Get CustomerModel from 'result'
            OkNegotiatedContentResult<CustomerModel> customerResult = (OkNegotiatedContentResult<CustomerModel>)result;


            //Act
            //The result of the Delete Request
             result = customersController.DeleteCustomer(customerResult.Content.CustomerId);

            //Assert

            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<CustomerModel>));         
        }

        [TestMethod]
        public void GetCustomerReturnCustomer()
        {
            //Arrange
            var customerController = new CustomersController();

            //Act
            IHttpActionResult result = customerController.GetCustomer(1);


            //Assert
            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            //If action returns: Ok()
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<CustomerModel>));

            //if action was returing data in the body like: Ok<string>("data: 12")
          //  OkNegotiatedContentResult<string> conNegResult = Assert.IsType<OkNegotiatedContentResult<string>>(actionResult);
          //  Assert.Equal("data: 12", conNegResult.Content);


        }

        [TestMethod]
        public void GetAccountsforCustomerIDReurnAccounts()
        {
            //Arrange
            var customerController = new CustomersController();

            //Act
            IHttpActionResult result = customerController.GetAccountsForCustomer(1);

            //Assert
            //If action returns: NotFound()
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            //If action returns: Ok()
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IQueryable<AccountModel>>));

        }
    }
}
