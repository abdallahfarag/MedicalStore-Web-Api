using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}