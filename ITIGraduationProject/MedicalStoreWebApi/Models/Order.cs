﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalStoreWebApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName ="Money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName ="Date")]
        public DateTime DateAdded { get; set; }
        [Required]
        public bool IsDelivered { get; set; }
        public string FeedBack { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User  { get; set; }

    }
}