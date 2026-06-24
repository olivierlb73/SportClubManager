using Microsoft.EntityFrameworkCore;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.UnitTests.TestHelpers;

internal static class ApplicationDbContextFactory
{
    public static ApplicationDbContext Create() => Create(Guid.NewGuid().ToString());

    /// <summary>
    /// Creates a context backed by the named in-memory database, so callers can open a second,
    /// untracked context against data seeded by a first one — mirroring separate scoped DbContext
    /// instances per request, which is what the handlers being tested are designed for.
    /// </summary>
    public static ApplicationDbContext Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new ApplicationDbContext(options);
    }
}
