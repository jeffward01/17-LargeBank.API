using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeBank.API.Models
{
    public class AccountModel
    {

        public int AccountId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int AccountNumber { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public decimal Balance { get; set; }

        public int TransactionsCount { get; set; }


        public string AllTransactionsUrl
        {
            get
            {
                return "api/transactions/" + AccountId +"/transactions/";
            }
        }

        public string CustomerURL
        {
            get
            {
                return "api/customers/" + CustomerId;
            }
        }


        
    }
}
