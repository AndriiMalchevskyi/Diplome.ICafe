using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Persistence
{
    class ICafeContextFactory : IDesignTimeDbContextFactory<ICafeContext>
    {
        public ICafeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ICafeContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ICafeDB");

            return new ICafeContext(optionsBuilder.Options);
        }
    }
}
