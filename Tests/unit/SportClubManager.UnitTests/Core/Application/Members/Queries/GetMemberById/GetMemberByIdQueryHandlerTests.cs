using SportClubManager.Core.Application.Members.Queries.GetMemberById;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.UnitTests.External.Infrastructure;

namespace SportClubManager.UnitTests.Core.Application.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandlerTests
{
    [Test]
    public async Task Handle_ReturnsMatchingMemberResponse()
    {
        using var context = ApplicationDbContextFactory.Create();
        var member = new Member("Jane", "Doe", "jane.doe@example.com");
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var handler = new GetMemberByIdQueryHandler(context);
        var query = new GetMemberByIdQuery(member.Id);

        var response = await handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.EqualTo(member.Id));
            Assert.That(response.FirstName, Is.EqualTo("Jane"));
            Assert.That(response.LastName, Is.EqualTo("Doe"));
        });
    }

    [Test]
    public void Handle_UnknownMemberId_ThrowsMemberNotFoundException()
    {
        using var context = ApplicationDbContextFactory.Create();
        var handler = new GetMemberByIdQueryHandler(context);
        var query = new GetMemberByIdQuery(404);

        Assert.ThrowsAsync<MemberNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }
}
