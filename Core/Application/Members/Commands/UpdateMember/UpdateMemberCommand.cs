using MediatR;

namespace SportClubManager.Core.Application.Members.Commands.UpdateMember;

public sealed record UpdateMemberCommand(long Id, string FirstName, string LastName, string Email) : IRequest;