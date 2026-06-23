using MediatR;
using Microsoft.EntityFrameworkCore;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.Core.Application.Members.Queries.GetMembers;

internal sealed class GetMembersQueryHandler(ApplicationDbContext context) : IRequestHandler<GetMembersQuery, IEnumerable<MemberResponse>>
{

    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<MemberResponse>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Members.Select(m => new MemberResponse(m.Id, m.FirstName, m.LastName))
                                     .ToListAsync();
    }
}