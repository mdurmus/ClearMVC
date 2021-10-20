namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class UserDetails
    {
        public int UserDetailsId { get; set; }

        public int? UserId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string GSM { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreateDate { get; set; }
        public virtual Users Users { get; set; }
    }
}
