using MediatR;

namespace SportClubManager.Core.Application.Members.Queries.GetMembers;

public sealed record GetMembersQuery() : IRequest<IEnumerable<MemberResponse>>;