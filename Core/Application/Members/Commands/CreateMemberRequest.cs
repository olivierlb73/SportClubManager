using MediatR;

namespace SportClubManager.Core.Application.Members.Commands;

public sealed record CreateMemberRequest(string FirstName, string LastName, string Email);