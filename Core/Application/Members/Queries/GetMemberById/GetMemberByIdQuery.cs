using MediatR;

namespace SportClubManager.Core.Application.Members.Queries.GetMemberById;

public sealed record GetMemberByIdQuery(long Id) : IRequest<MemberResponse>;