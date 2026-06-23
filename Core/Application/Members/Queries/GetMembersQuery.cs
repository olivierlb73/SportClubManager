using MediatR;

namespace SportClubManager.Core.Application.Members.Queries;

public sealed record GetMembersQuery() : IRequest<IEnumerable<MemberResponse>>;