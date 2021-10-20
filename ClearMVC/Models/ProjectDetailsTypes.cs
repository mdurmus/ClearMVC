namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ProjectDetailsTypes
    {
        public ProjectDetailsTypes()
        {
            ProjectDetails = new HashSet<ProjectDetails>();
        }

        public int ProjectDetailsTypesId { get; set; }

        [StringLength(500)]
        public string Type { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

       
        public int FirmaId { get; set; }
        public virtual Firma Firma { get; set; }


        public virtual ICollection<ProjectDetails> ProjectDetails { get; set; }
    }
}
