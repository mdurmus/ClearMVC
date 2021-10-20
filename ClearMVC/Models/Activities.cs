namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Activities
    {
        [Key]
        public int ActivitiyId { get; set; }

        public int? FirmaId { get; set; }

        [StringLength(1000)]
        public string Subject { get; set; }

        public string ActivityText { get; set; }

        public bool ForCustomer { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ActivityDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

        public virtual Firma Firma { get; set; }
    }
}
