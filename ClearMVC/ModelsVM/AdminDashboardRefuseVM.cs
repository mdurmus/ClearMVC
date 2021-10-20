using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.ModelsVM
{
    public class AdminDashboardRefuseVM
    {
        public int ProjectId { get; set; }
        public int ProjectdetailId { get; set; }
        public string Customer { get; set; }
        public string ProjectName { get; set; }
        public string Employee { get; set; }

    }
}