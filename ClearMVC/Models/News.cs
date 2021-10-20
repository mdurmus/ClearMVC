namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class News
    {
        public int NewsId { get; set; }

        public int? FirmaId { get; set; }

        [StringLength(1000)]
        public string Subject { get; set; }

        public bool ForCustomer { get; set; }

        public string Text { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreateDate { get; set; }

        public virtual Firma Firma { get; set; }
    }
}
