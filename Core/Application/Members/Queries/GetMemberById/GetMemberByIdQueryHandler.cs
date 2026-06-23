using MediatR;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.Core.Application.Members.Queries.GetMemberById;

internal sealed class GetMemberByIdQueryHandler(ApplicationDbContext context) : IRequestHandler<GetMemberByIdQuery, MemberResponse>
{

    private readonly ApplicationDbContext _context = context;

    public async Task<MemberResponse> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Id, cancellationToken) ?? throw new MemberNotFoundException(request.Id);

        return new MemberResponse(member.Id, member.FirstName, member.LastName);
    }
}