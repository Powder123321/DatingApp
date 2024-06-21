using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API;

public class DataContext :DbContext
{
  

  public DataContext(DbContextOptions options) :base(options)
  {
    
  }
public DbSet<AppUser> Users { get; set; }

    internal async Task<AppUser> FindAsync(int id)
    {
        throw new NotImplementedException();
    }
}
