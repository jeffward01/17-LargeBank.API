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


        public string TransactionsUrl
        {
            get
            {
                return "api/Transactions/" + AccountId;
            }
        }
    }
}
