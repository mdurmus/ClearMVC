using ClearMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.ModelsVM
{
    public class UserVM
    {
        public virtual Users Users { get; set; }
        public virtual UserDetails UserDetails { get; set; }
    }
}