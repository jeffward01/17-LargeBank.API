using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeBank.API.Models
{
   public class TransactionModel
    {

        public int TransactionId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> Amount { get; set; }

        public string AccountsUrl
        {
            get
            {
                return "api/accounts/" + AccountId;
            }
        }
        public string SingleTransactionUrl
        {
            get
            {
                return "api/transactions/" + TransactionId; ;
            }

        }
    }
}
