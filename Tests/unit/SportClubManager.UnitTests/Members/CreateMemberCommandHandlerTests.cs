using SportClubManager.Core.Application.Members.Commands.CreateMember;
using SportClubManager.UnitTests.TestHelpers;

namespace SportClubManager.UnitTests.Members;

public class CreateMemberCommandHandlerTests
{
    [Test]
    public async Task Handle_PersistsMemberAndReturnsGeneratedId()
    {
        using var context = ApplicationDbContextFactory.Create();
        var handler = new CreateMemberCommandHandler(context);
        var command = new CreateMemberCommand("Jane", "Doe", "jane.doe@example.com");

        var id = await handler.Handle(command, CancellationToken.None);

        Assert.That(id, Is.GreaterThan(0));
        var stored = context.Members.Single(m => m.Id == id);
        Assert.Multiple(() =>
        {
            Assert.That(stored.FirstName, Is.EqualTo("Jane"));
            Assert.That(stored.LastName, Is.EqualTo("Doe"));
            Assert.That(stored.Email, Is.EqualTo("jane.doe@example.com"));
        });
    }
}
