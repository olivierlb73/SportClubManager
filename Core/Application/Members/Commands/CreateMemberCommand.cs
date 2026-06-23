using MediatR;

namespace SportClubManager.Core.Application.Members.Commands;

public sealed record CreateMemberCommand(string FirstName, string LastName, string Email) : IRequest<long?>;