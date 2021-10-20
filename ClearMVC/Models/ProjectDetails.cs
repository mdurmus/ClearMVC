namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ProjectDetails
    {
        public int ProjectDetailsId { get; set; }

        public int? ProjectId { get; set; }

        public int? PersonId { get; set; }

        public int? ProjectDetailTypeId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FinishDate { get; set; }

        public int? Square { get; set; }

        public double? Duration { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsCompleted { get; set; }

        public bool IsRefuse { get; set; }
        public bool ForRefuse { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CompleteDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public bool IsCloseRefuseByCustomer { get; set; } = false;

        public virtual ProjectDetailsTypes ProjectDetailsTypes { get; set; }

        public virtual Projects Projects { get; set; }

        public virtual Users Users { get; set; }

        public virtual ICollection<Refuse> Refuses { get; set; }
    }
}
