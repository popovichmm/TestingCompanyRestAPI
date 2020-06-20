using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyREST.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string INN { get; set; }
        public string CPP { get; set; }
        [Required]
        public string Address { get; set; }

        public List<Department> Departments { get; set; }
    }
}
