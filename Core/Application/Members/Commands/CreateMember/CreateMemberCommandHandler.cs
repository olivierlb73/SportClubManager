using MediatR;
using Microsoft.EntityFrameworkCore;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.Core.Application.Members.Commands.CreateMember;

internal sealed class CreateMemberCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateMemberCommand, long?>
{

    private readonly ApplicationDbContext _context = context;

    public async Task<long?> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        var member = new Member(command.FirstName, command.LastName, command.Email);

        _context.Members.Add(member);

        await _context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}