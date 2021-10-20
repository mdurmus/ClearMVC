using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.ModelsVM
{
    public class DailyJobCustomerVM
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public int JobCount { get; set; }

    }
}