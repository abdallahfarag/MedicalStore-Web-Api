using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalStoreWebApi.Models
{
    public class Cart
    {
        [Key]
        [Column(Order =0)]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Key]
        [Column(Order =1)]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; } 
    }
}