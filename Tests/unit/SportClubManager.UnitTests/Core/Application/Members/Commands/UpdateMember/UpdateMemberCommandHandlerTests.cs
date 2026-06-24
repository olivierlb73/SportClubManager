using SportClubManager.Core.Application.Members.Commands.UpdateMember;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.UnitTests.External.Infrastructure;

namespace SportClubManager.UnitTests.Core.Application.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandlerTests
{
    [Test]
    public async Task Handle_UpdatesExistingMemberFields()
    {
        var databaseName = Guid.NewGuid().ToString();
        long memberId;
        using (var seedContext = ApplicationDbContextFactory.Create(databaseName))
        {
            var member = new Member("Jane", "Doe", "jane.doe@example.com");
            seedContext.Members.Add(member);
            await seedContext.SaveChangesAsync();
            memberId = member.Id;
        }

        using var context = ApplicationDbContextFactory.Create(databaseName);
        var handler = new UpdateMemberCommandHandler(context);
        var command = new UpdateMemberCommand(memberId, "Janet", "Smith", "janet.smith@example.com");

        await handler.Handle(command, CancellationToken.None);

        var updated = context.Members.Single(m => m.Id == memberId);
        Assert.Multiple(() =>
        {
            Assert.That(updated.FirstName, Is.EqualTo("Janet"));
            Assert.That(updated.LastName, Is.EqualTo("Smith"));
            Assert.That(updated.Email, Is.EqualTo("janet.smith@example.com"));
        });
    }

    [Test]
    public void Handle_UnknownMemberId_ThrowsMemberNotFoundException()
    {
        using var context = ApplicationDbContextFactory.Create();
        var handler = new UpdateMemberCommandHandler(context);
        var command = new UpdateMemberCommand(404, "Jane", "Doe", "jane.doe@example.com");

        Assert.ThrowsAsync<MemberNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
