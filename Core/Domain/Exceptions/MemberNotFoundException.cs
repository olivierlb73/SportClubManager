namespace SportClubManager.Core.Domain.Exceptions;

public sealed class MemberNotFoundException(long memberId) : Exception($"The member with the identifier {memberId} was not found.")
{
}