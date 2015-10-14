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
    public class AccountsController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        // GET: api/Accounts
        public IQueryable<AccountModel> GetAccounts()
        {
            //LINQ statement Projecting source to JSON

            //Select method to return all CustomerModel from Customers class
            return db.Accounts.Select(c => new AccountModel
            {
                AccountId = c.AccountId,
                AccountNumber = c.AccountNumber,
                Balance = c.Balance,
                CreatedDate = c.CreatedDate,
                CustomerId = c.CustomerId,         
            });
        }

        // GET: api/Accounts/5
        [ResponseType(typeof(AccountModel))]
        public IHttpActionResult GetAccount(int id)
        {
            Account dbAccount = db.Accounts.Find(id);

            if (dbAccount == null)
            {
                return NotFound();
            }

            AccountModel modelAccount = new AccountModel
            {
                AccountId = dbAccount.AccountId,
                AccountNumber = dbAccount.AccountNumber,
                Balance = dbAccount.Balance,
                CreatedDate = dbAccount.CreatedDate,
                CustomerId = dbAccount.CustomerId,
            };
            return Ok(modelAccount);
        }

        // PUT: api/Accounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount(int id, AccountModel account)
        {
            //this checks is everything is good with the request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.CustomerId)
            {
                return BadRequest();
            }

            //Mark the object as Modified
            //Update customer in the database
            var dbAccount = db.Accounts.Find(id);

            //update the Database
            dbAccount.Update(account);
                

            //Updates Entries STATE in the Database
            db.Entry(dbAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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


        // POST: api/Accounts
        [ResponseType(typeof(AccountModel))]
        public IHttpActionResult PostAccount(AccountModel account)
        {
            //If everything is good with the communication
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Build new Account
            var dbAccount = new Account();

            //Update Customer with new Model
            dbAccount.Update(account);

            //add Customer model to DB
            db.Accounts.Add(dbAccount);

            //Updates Entries STATE in the Database
            db.Entry(dbAccount).State = EntityState.Modified;

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = account.CustomerId }, account);
        }

        // DELETE: api/Accounts/5
        [ResponseType(typeof(AccountModel))]
        public IHttpActionResult DeleteAccount(int id)
        {
            //Locate Customer from Database
            Account account = db.Accounts.Find(id);

            //If Customer is not found in Database
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);

            //Updates Entries STATE in the Database
            db.Entry(account).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.AccountId == id) > 0;
        }
    }
}