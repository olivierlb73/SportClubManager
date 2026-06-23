using MediatR;
using Microsoft.EntityFrameworkCore;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.Core.Application.Members.Commands;

internal sealed class UpdateMemberCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateMemberCommand>
{

    private readonly ApplicationDbContext _context = context;

    public async Task Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        var member = new Member(command.Id, command.FirstName, command.LastName, command.Email);

        _context.Entry(member).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MemberExists(command.Id))
            {
                throw new MemberNotFoundException(command.Id);
            }
            else
            {
                throw;
            }
        }
    }

    private bool MemberExists(long id)
    {
        return _context.Members.Any(e => e.Id == id);
    }
}