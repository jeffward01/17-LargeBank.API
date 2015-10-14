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

            //Updates Entries STATE in the Database
            db.Entry(dbCustomer).State = EntityState.Modified;

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
        }

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

            db.Customers.Remove(customer);

            //Updates Entries STATE in the Database
            db.Entry(customer).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(customer);
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