using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LargeBank.API.Controllers;
using System.Collections.Generic;
using LargeBank.API.Models;
using System.Linq;

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


    }
}
