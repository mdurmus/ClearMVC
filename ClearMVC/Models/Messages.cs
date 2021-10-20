namespace ClearMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class Messages
    {
        public int MessagesId { get; set; }

        public int? UserId { get; set; }

        public string MessageText { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LeaveDate { get; set; }

        [StringLength(10)]
        public string Sender { get; set; }

        public virtual Users Users { get; set; }
    }
}
