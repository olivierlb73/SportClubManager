using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportClubManager.Core.Application.Members.Commands;
using SportClubManager.Core.Application.Members.Queries;
using SportClubManager.Core.Domain.Exceptions;
using SportClubManager.Core.Domain.Models;
using SportClubManager.External.Infrastructure;

namespace SportClubManager.External.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembers(CancellationToken cancellationToken)
        {
            var query = new GetMembersQuery();

            var members = await _sender.Send(query, cancellationToken);

            return Ok(members);
        }

        // GET: api/Members/5
        [HttpGet("{memberId}")]
        public async Task<ActionResult<MemberResponse>> GetMember(long memberId, CancellationToken cancellationToken)
        {
            var query = new GetMemberByIdQuery(memberId);

            try
            {
                var member = await _sender.Send(query, cancellationToken);

                return Ok(member);
            }
            catch (MemberNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(long id, UpdateMemberRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateMemberCommand(id, request.FirstName, request.LastName, request.Email);

            try
            {
                await _sender.Send(command, cancellationToken);
            }
            catch (MemberNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(CreateMemberRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateMemberCommand>();

            var memberId = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetMember), new { memberId }, memberId);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(long id, CancellationToken cancellationToken)
        {
            var command = new DeleteMemberCommand(id);

            try
            {
                await _sender.Send(command, cancellationToken);
            }
            catch (MemberNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
