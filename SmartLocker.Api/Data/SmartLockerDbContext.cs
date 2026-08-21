using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Models;

namespace SmartLocker.Api.Data;

public class SmartLockerDbContext : DbContext
{
    public SmartLockerDbContext(
        DbContextOptions<SmartLockerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Locker> Lockers => Set<Locker>();
}