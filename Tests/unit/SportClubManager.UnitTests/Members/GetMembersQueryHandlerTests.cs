using SportClubManager.Core.Application.Members.Queries.GetMembers;
using SportClubManager.Core.Domain.Models;
using SportClubManager.UnitTests.TestHelpers;

namespace SportClubManager.UnitTests.Members;

public class GetMembersQueryHandlerTests
{
    [Test]
    public async Task Handle_ReturnsAllMembersAsResponses()
    {
        using var context = ApplicationDbContextFactory.Create();
        context.Members.AddRange(
            new Member("Jane", "Doe", "jane.doe@example.com"),
            new Member("John", "Smith", "john.smith@example.com"));
        await context.SaveChangesAsync();

        var handler = new GetMembersQueryHandler(context);

        var responses = await handler.Handle(new GetMembersQuery(), CancellationToken.None);

        Assert.That(responses.Select(r => r.FirstName), Is.EquivalentTo(new[] { "Jane", "John" }));
    }

    [Test]
    public async Task Handle_NoMembers_ReturnsEmptyCollection()
    {
        using var context = ApplicationDbContextFactory.Create();
        var handler = new GetMembersQueryHandler(context);

        var responses = await handler.Handle(new GetMembersQuery(), CancellationToken.None);

        Assert.That(responses, Is.Empty);
    }
}
