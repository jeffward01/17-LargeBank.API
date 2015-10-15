using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LargeBank.API;
using LargeBank.API.Models;

namespace LargeBank.API.Models
{
    public class CustomerModel
    {

        public int CustomerId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string States { get; set; }
        public string Zip { get; set; }

        public string AccountsUrl
        {
            get
            {
                return "api/customers/" + CustomerId + "/accounts/";
            }
        }


    }
}
