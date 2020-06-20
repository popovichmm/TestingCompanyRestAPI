using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyREST.Contexts
{
    public static class SeedData
    {        
        public static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db_CompanyREST = serviceScope.ServiceProvider.GetService<CompanyDbContext>();
                db_CompanyREST.Database.Migrate();
            }
        }
    }
}
