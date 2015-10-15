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
    public class UnitTest1
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
    }
}
