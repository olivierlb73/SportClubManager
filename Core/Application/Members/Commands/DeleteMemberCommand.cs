using MediatR;

namespace SportClubManager.Core.Application.Members.Commands;

public sealed record DeleteMemberCommand(long MemberId) : IRequest;