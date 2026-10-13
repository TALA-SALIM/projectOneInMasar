using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework.Models
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}