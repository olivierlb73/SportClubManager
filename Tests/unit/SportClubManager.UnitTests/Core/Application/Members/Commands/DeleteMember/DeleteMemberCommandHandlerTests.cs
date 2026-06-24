using SportClubManager.Core.Application.Members.Commands.DeleteMember;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.UnitTests.External.Infrastructure;

namespace SportClubManager.UnitTests.Core.Application.Members.Commands.DeleteMember;

public class DeleteMemberCommandHandlerTests
{
    [Test]
    public async Task Handle_RemovesExistingMember()
    {
        using var context = ApplicationDbContextFactory.Create();
        var member = new Member("Jane", "Doe", "jane.doe@example.com");
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var handler = new DeleteMemberCommandHandler(context);
        var command = new DeleteMemberCommand(member.Id);

        await handler.Handle(command, CancellationToken.None);

        Assert.That(context.Members.Any(m => m.Id == member.Id), Is.False);
    }

    [Test]
    public void Handle_UnknownMemberId_ThrowsMemberNotFoundException()
    {
        using var context = ApplicationDbContextFactory.Create();
        var handler = new DeleteMemberCommandHandler(context);
        var command = new DeleteMemberCommand(404);

        Assert.ThrowsAsync<MemberNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
