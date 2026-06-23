using MediatR;

namespace SportClubManager.Core.Application.Members.Commands;

public sealed record UpdateMemberRequest(string FirstName, string LastName, string Email);