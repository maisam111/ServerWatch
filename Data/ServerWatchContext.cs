using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerWatch.Models;

namespace ServerWatch.Data
{
    public class ServerWatchContext : DbContext
    {
        public ServerWatchContext (DbContextOptions<ServerWatchContext> options)
            : base(options)
        {
        }

        public DbSet<staticInfo> staticInfo { get; set; }
    }
}
