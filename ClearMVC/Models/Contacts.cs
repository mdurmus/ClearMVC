namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Contacts
    {
        public int ContactsId { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string SurName { get; set; }

        [StringLength(20)]
        public string GSM { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
