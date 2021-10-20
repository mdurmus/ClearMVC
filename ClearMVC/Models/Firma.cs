using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.Models
{
    public class Firma
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VATCode { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDatetime { get; set; } = DateTime.Now;

        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Projects> Projects { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<ProjectDetailsTypes> ProjectDetailsTypes { get; set; }

    }
}