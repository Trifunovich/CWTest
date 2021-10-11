using DataAccess.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Sql.Context
{
  class SqlEfContext : DbContext
  {
    private readonly ISqlConnectionFactory _connectionFactory;

    public SqlEfContext(DbContextOptions options, ISqlConnectionFactory connectionFactory) : base(options)
    {
      _connectionFactory = connectionFactory;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      var connectionString = _connectionFactory?.CreateSqlDbConnection()?.ConnectionString;
      if (connectionString != null)
        optionsBuilder.UseSqlite(connectionString);
    }

    public DbSet<ComponentSpecification> ComponentSpecifications { get; set; }

  }
}
