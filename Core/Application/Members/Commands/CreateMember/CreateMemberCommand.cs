using MediatR;

namespace SportClubManager.Core.Application.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(string FirstName, string LastName, string Email) : IRequest<long>;