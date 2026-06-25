using System.Collections.ObjectModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Moq;
using SportClubManager.Core.Application.Members.Commands.CreateMember;
using SportClubManager.Core.Application.Members.Commands.DeleteMember;
using SportClubManager.Core.Application.Members.Commands.UpdateMember;
using SportClubManager.Core.Application.Members.Queries;
using SportClubManager.Core.Application.Members.Queries.GetMemberById;
using SportClubManager.Core.Application.Members.Queries.GetMembers;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.External.Presentation.Controllers;

namespace SportClubManager.UnitTests.External.Presentation.Controllers;

[TestFixture]
public class MembersControllerTests
{
    private static readonly MemberResponse _testMember = new(1, "John", "Doe");
    private static ReadOnlyCollection<MemberResponse> _testMembers = [
            _testMember,
            new MemberResponse(2, "Jane", "Spike")
    ];

    [Test]
    public async Task GetMembers_ReturnsTheListOfMembers()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(_testMembers);
        var controller = new MembersController(senderMock.Object);

        // Act
        ActionResult<IEnumerable<MemberResponse>> result = await controller.GetMembers(CancellationToken.None);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(okResult?.Value, Is.EquivalentTo(_testMembers));
    }

    [Test]
    public async Task GetMember_ReturnsTheExpectedMember()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<GetMemberByIdQuery>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(_testMember);
        var controller = new MembersController(senderMock.Object);

        // Act
        ActionResult<MemberResponse> result = await controller.GetMember(_testMember.Id, CancellationToken.None);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(okResult?.Value, Is.EqualTo(_testMember));
    }

    [Test]
    public async Task GetMember_ReturnsNotFoundAfterMemberNotFoundException()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<GetMemberByIdQuery>(), It.IsAny<CancellationToken>()))
                                    .Throws(new MemberNotFoundException(_testMember.Id));
        var controller = new MembersController(senderMock.Object);

        // Act
        ActionResult<MemberResponse> result = await controller.GetMember(_testMember.Id, CancellationToken.None);

        // Assert
        var notFoundResult = result.Result as NotFoundResult;
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public async Task PutMember_ReturnsNoContentWhenUpdateSucceeds()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<UpdateMemberCommand>(), It.IsAny<CancellationToken>()))
                                    .Returns(Task.CompletedTask);
        var controller = new MembersController(senderMock.Object);
        var request = new UpdateMemberRequest("John", "Doe", "john.doe@example.com");

        // Act
        IActionResult result = await controller.PutMember(_testMember.Id, request, CancellationToken.None);

        // Assert
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult?.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public async Task PutMember_ReturnsNotFoundAfterMemberNotFoundException()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<UpdateMemberCommand>(), It.IsAny<CancellationToken>()))
                                    .ThrowsAsync(new MemberNotFoundException(_testMember.Id));
        var controller = new MembersController(senderMock.Object);
        var request = new UpdateMemberRequest("John", "Doe", "john.doe@example.com");

        // Act
        IActionResult result = await controller.PutMember(_testMember.Id, request, CancellationToken.None);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public async Task PostMember_ReturnsCreatedAtActionWithNewMemberId()
    {
        // Arrange
        const long newMemberId = 42;
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<CreateMemberCommand>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(newMemberId);
        var controller = new MembersController(senderMock.Object);
        var request = new CreateMemberRequest("John", "Doe", "john.doe@example.com");

        // Act
        var result = await controller.PostMember(request, CancellationToken.None);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        Assert.That(createdAtActionResult?.StatusCode, Is.EqualTo(201));
        Assert.That(createdAtActionResult?.ActionName, Is.EqualTo(nameof(MembersController.GetMember)));
        Assert.That(createdAtActionResult?.Value, Is.EqualTo(newMemberId));
    }

    [Test]
    public async Task DeleteMember_ReturnsNoContentWhenDeleteSucceeds()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<DeleteMemberCommand>(), It.IsAny<CancellationToken>()))
                                    .Returns(Task.CompletedTask);
        var controller = new MembersController(senderMock.Object);

        // Act
        IActionResult result = await controller.DeleteMember(_testMember.Id, CancellationToken.None);

        // Assert
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult?.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public async Task DeleteMember_ReturnsNotFoundAfterMemberNotFoundException()
    {
        // Arrange
        var senderMock = new Mock<ISender>();
        senderMock.Setup(sender => sender
                                    .Send(It.IsAny<DeleteMemberCommand>(), It.IsAny<CancellationToken>()))
                                    .ThrowsAsync(new MemberNotFoundException(_testMember.Id));
        var controller = new MembersController(senderMock.Object);

        // Act
        IActionResult result = await controller.DeleteMember(_testMember.Id, CancellationToken.None);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }
}