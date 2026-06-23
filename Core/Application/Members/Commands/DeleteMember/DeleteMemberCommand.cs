using MediatR;

namespace SportClubManager.Core.Application.Members.Commands.DeleteMember;

public sealed record DeleteMemberCommand(long MemberId) : IRequest;