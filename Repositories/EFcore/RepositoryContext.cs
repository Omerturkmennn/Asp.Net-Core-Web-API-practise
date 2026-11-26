using Entities1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.EFcore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFcore
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new BookConfig());
            //modelBuilder.ApplyConfiguration(new RoleConfig());  
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//yukardakılerı tek tek girmek yerine bu yeterli
        }
    }
}
