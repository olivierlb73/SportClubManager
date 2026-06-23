using MediatR;

namespace SportClubManager.Core.Application.Members.Queries;

public sealed record GetMemberByIdQuery(long Id) : IRequest<MemberResponse>;