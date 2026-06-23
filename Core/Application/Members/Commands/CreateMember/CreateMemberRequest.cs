using MediatR;

namespace SportClubManager.Core.Application.Members.Commands.CreateMember;

public sealed record CreateMemberRequest(string FirstName, string LastName, string Email);