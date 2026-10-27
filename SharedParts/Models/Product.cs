using SharedParts.Shareds.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParts.Models
{
    public class Product : Entity
    { 
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
         
    }
}
