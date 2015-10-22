using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LargeBank.API;
using LargeBank.API.Models;

namespace LargeBank.API.Controllers
{
    public class CustomersController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        //This gets all accounts for a customerId
        //GET: api/Customers/5
        [Route("api/customers/{id}/accounts")]
        public IHttpActionResult GetAccountsForCustomer(int id)
        {
            var customersAccount = db.Accounts.Where(a => a.CustomerId == id);

            //Error checking

            return Ok(customersAccount.Select(a => new AccountModel
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                Balance = a.Balance,
                CreatedDate = a.CreatedDate,
                CustomerId = a.CustomerId
            }));
        }

        //This gets a list of all Customers
        // GET: api/Customers
        public IQueryable<CustomerModel> GetCustomers()
        {
            //LINQ statement Projecting source to JSON

            //Select method to return all CustomerModel from Customers class
            return db.Customers.Select(c => new CustomerModel
            {
                CustomerId = c.CustomerId,
                Address1 = c.Address1,
                Address2 = c.Address2,
                City = c.City,
                CreatedDate = c.CreatedDate,
                FirstName = c.FirstName,
                LastName = c.LastName,
                States = c.States,
                Zip = c.Zip            
            });
        }

        //This gets Data from a Customer
        // GET: api/Customers/5
        [ResponseType(typeof(CustomerModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer dbCustomer = db.Customers.Find(id);

            if (dbCustomer == null)
            {
                return NotFound();
            }

            CustomerModel modelCustomer = new CustomerModel
            {
                CustomerId = dbCustomer.CustomerId,
                Address1 = dbCustomer.Address1,
                Address2 = dbCustomer.Address2,
                City = dbCustomer.City,
                CreatedDate = dbCustomer.CreatedDate,
                FirstName = dbCustomer.FirstName,
                LastName = dbCustomer.LastName,
                States = dbCustomer.States,
                Zip = dbCustomer.Zip
            };
           
        
            return Ok(modelCustomer);
        }

        //THis updates A Customer
        //Update Customer
        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerModel customer)
        {

            //this checks is everything is good with the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            //Mark the object as Modified
            //Update customer in the database
            var dbCustomer = db.Customers.Find(id);

            //update the Database
            dbCustomer.Update(customer);

            //Updates Entries STATE in the Database
            db.Entry(dbCustomer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //This creates a new Cusomter
        //Create new Customer
        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(CustomerModel customer)
        {
            //If everything is good with the communication
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Build new Customer
            var dbCustomer = new Customer();

            //Update Customer with new Model
            dbCustomer.Update(customer);
            
            //add Customer model to DB
            db.Customers.Add(dbCustomer);         

            db.SaveChanges();

            customer.CustomerId = dbCustomer.CustomerId;

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
        }

        //This deletes a customer
        // DELETE: api/Customers/5
        [ResponseType(typeof(CustomerModel))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            //Locate Customer from Database
            Customer customer = db.Customers.Find(id);

            //If Customer is not found in Database
            if (customer == null)
            {
                return NotFound();
            }

            var transactions = db.Transactions.Where(t => t.Account.CustomerId == customer.CustomerId);

            db.Transactions.RemoveRange(transactions);

            db.SaveChanges();

            var accounts = db.Accounts.Where(a => a.CustomerId == customer.CustomerId);

            db.Accounts.RemoveRange(accounts);


            db.SaveChanges();

            db.Customers.Remove(customer);

      
            db.SaveChanges();

            //Return model to user
            var customerModel = new CustomerModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                City = customer.City,
                States = customer.States,
                Zip = customer.Zip,
            };

            return Ok(customerModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}