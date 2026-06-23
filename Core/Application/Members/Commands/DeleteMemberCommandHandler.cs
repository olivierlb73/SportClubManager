using MediatR;
using Microsoft.EntityFrameworkCore;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.Core.Application.Members.Commands;

internal sealed class DeleteMemberCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteMemberCommand>
{

    private readonly ApplicationDbContext _context = context;

    public async Task Handle(DeleteMemberCommand command, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(command.MemberId) ?? throw new MemberNotFoundException(command.MemberId);

        _context.Members.Remove(member);

        await _context.SaveChangesAsync();
    }
}