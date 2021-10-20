using ClearMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.ModelsVM
{
    public class ProjectDetailVM
    {
        public Projects Project { get; set; }

        public Customers Customer { get; set; }

        public Contacts Contact { get; set; }


    }
}