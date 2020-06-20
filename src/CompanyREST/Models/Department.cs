using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyREST.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CountEmployees { get; set; }
        
        public Company Company { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
