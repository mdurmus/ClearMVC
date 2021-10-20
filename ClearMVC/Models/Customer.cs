using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.Models
{
    public class Customers
    {
        public int CustomersId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Strasse { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Area { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? FirmaId { get; set; }
        public virtual Firma Firma { get; set; }


        public virtual ICollection<Projects> Projects { get; set; }
      
    }
}