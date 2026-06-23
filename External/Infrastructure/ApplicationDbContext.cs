using Microsoft.EntityFrameworkCore;
using SportClubManager.Core.Domain.Models;

namespace SportClubManager.External.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Member> Members { get; set; } = null!;
}