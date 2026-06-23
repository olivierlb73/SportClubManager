using MediatR;

namespace SportClubManager.Core.Application.Members.Commands.UpdateMember;

public sealed record UpdateMemberRequest(string FirstName, string LastName, string Email);