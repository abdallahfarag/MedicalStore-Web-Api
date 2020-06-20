using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalStoreWebApi.Models
{
    public class OrderItems
    {
        [Key]
        [Column(Order =0)]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [Key]
        [Column(Order =1)]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}