using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.ModelsVM
{
    public class DailyJobVM
    {
        public int Id { get; set; }
        public int ProjectDetailId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Location { get; set; }
         public double Duration { get; set; }
        public string CustomerName { get; set; }

        public bool ForRefuse { get; set; }
    }
}