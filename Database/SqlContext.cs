using ServerAuth.Models;
using ServerWatch.Models;
using System.Data.Entity;

public class SqlContext : DbContext
{

    public virtual DbSet<staticInfo> Info { get; set; }
}