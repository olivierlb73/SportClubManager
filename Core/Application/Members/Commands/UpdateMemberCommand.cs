using MediatR;

namespace SportClubManager.Core.Application.Members.Commands;

public sealed record UpdateMemberCommand(long Id, string FirstName, string LastName, string Email) : IRequest;