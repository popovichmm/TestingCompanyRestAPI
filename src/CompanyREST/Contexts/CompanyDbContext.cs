using CompanyREST.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyREST.Contexts
{    
    public class CompanyDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
                    : base(options)
        {

        }
    }    
}
