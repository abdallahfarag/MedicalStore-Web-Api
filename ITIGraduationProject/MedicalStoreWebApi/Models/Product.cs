using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalStoreWebApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int QuantityInStock { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}