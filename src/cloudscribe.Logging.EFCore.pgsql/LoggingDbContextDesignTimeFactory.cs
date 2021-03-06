﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Logging.EFCore.pgsql
{
    public class LoggingDbContextDesignTimeFactory : IDesignTimeDbContextFactory<LoggingDbContext>
    {
        public LoggingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LoggingDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");

            return new LoggingDbContext(builder.Options);
        }
    }
}
