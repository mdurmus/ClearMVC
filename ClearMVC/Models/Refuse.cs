using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearMVC.Models
{
    public class Refuse
    {
        public int RefuseId { get; set; }
        public int ProjectId { get; set; }
        public string RefuseText { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LeaveDate { get; set; } = DateTime.Now;

        public int ProjectDetailsId { get; set; }
        public virtual ProjectDetails ProjectDetails { get; set; }
    }
}