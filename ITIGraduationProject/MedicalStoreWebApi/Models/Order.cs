using MedicalStoreWebApi.Models.Enums;
using Newtonsoft.Json;
using System;
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
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }
        [Required]
        public Orderstatus OrderStatus { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public string OrderAddress { get; set; }
        [Phone]
        [Required]
        public string ContactPhone { get; set; }
        public string FeedBack { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User  { get; set; }
        
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new HashSet<OrderItems>();

    }
}