namespace SportClubManager.Core.Domain.Models;

public class Member(long id, string firstName, string lastName, string email)
{
    public Member(string firstName, string lastName, string email)
        : this(0, firstName, lastName, email)
    {
    }

    public long Id { get; init; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
}