using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParts.Shareds.BaseEntity
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Entity()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
