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
    public class TransactionsController : ApiController
    {
        private LargeBankEntities db = new LargeBankEntities();

        // GET: api/Transactions
        public IQueryable<TransactionModel> GetTransactions()
        {
            //LINQ statement Projecting source to JSON

            //Select method to return all CustomerModel from Customers class
            return db.Transactions.Select(c => new TransactionModel
            {
                TransactionDate = c.TransactionDate,
                TransactionId = c.TransactionId,
                Amount = c.Amount,
                AccountId = c.AccountId,
              
            });
        }

        
        // GET: api/transactions/5
      [ResponseType(typeof(TransactionModel))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            TransactionModel modelTransaction = new TransactionModel
            {
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                AccountId = transaction.AccountId
            };

            return Ok(modelTransaction);
        }
        

        //Gets ALL transacitons for an accountId.
        public IHttpActionResult GetTransactions(int accountId)
        {
            var transaction = db.Transactions.Where(a => a.AccountId == accountId);
            if (transaction == null)
            {
                return NotFound();
            }

            //LINQ statement Projecting source to JSON
            //Select method to return all CustomerModel from Customers class
            return Ok(transaction.Select(c => new TransactionModel
            {
                AccountId = c.AccountId,
                TransactionId = c.TransactionId,
                TransactionDate = c.TransactionDate,
                Amount = c.Amount,
            }));
        }

        /*
        Not Needed Method
        //Gets Single Transaction from transactionId
        public IHttpActionResult GetSingleTransaction(int TransactionId)
        {
           var transaction = db.Transactions.Find(TransactionId);
            if (transaction == null)
            {
                return NotFound();
            }



            var modelTransactions = new TransactionModel
            {
                AccountId = transaction.AccountId,
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
            };


            //Select method to return all CustomerModel from Customers class
            return Ok(modelTransactions);
        }
        */
        //Update
        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, TransactionModel transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            //Mark the object as Modified
            //Update customer in the database
            var dbTransaction = db.Transactions.Find(id);

            //update the Database
            dbTransaction.Update(transaction);


            //Updates Entries STATE in the Database
            db.Entry(dbTransaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        //Create
        // POST: api/Transactions
        [ResponseType(typeof(TransactionModel))]
        public IHttpActionResult PostTransaction(TransactionModel transaction)
        {
            //If everything is good with the communication
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Build new Account
            var dbTransaction = new Transaction();

            //Update Customer with new Model
            dbTransaction.Update(transaction);

            //add Customer model to DB
            db.Transactions.Add(dbTransaction);

            //Updates Entries STATE in the Database
            db.Entry(dbTransaction).State = EntityState.Modified;

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(TransactionModel))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.TransactionId == id) > 0;
        }
    }
}