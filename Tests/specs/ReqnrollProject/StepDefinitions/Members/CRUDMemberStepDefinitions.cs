using SportClubManager.Core.Application.Members.Commands.CreateMember;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity.Data;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;
using NuGet.Protocol;
using SportClubManager.Core.Application.Members.Queries.GetMemberById;
using SportClubManager.Core.Application.Members.Queries;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ReqnrollProject.StepDefinitions.Members;

[Binding]
public class CRUDMemberStepDefinitions
{
    private readonly HttpClient _httpClient;
    private long _memberId;
    private MemberResponse? _expectedMemberResponse;

    public CRUDMemberStepDefinitions(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
    }

    [When("I create a new member with")]
    public async Task ICreateANewMemberWith(Table table)
    {
        var row = table.Rows[0];
        CreateMemberCommand createMemberCommand = new (row["firstName"], row["lastName"], row["email"]);
            
        var result = await _httpClient.PostAsJsonAsync("/api/members", createMemberCommand);

        result.EnsureSuccessStatusCode();
        _memberId = await result.Content.ReadFromJsonAsync<long>();
        _expectedMemberResponse = new MemberResponse(_memberId, row["firstName"], row["lastName"]);
    }

    [Then("I can retrieve the new member")]
    public async Task ICanRetrieveTheNewMember()
    {
      var memberResponse = await _httpClient.GetAsync($"/api/members/{_memberId}");
      memberResponse.EnsureSuccessStatusCode();

      var member = await memberResponse.Content.ReadFromJsonAsync<MemberResponse>();

      Assert.That(member, Is.Not.Null);
      Assert.That(member!.Id, Is.EqualTo(_expectedMemberResponse!.Id));
      Assert.That(member.FirstName, Is.EqualTo(_expectedMemberResponse.FirstName));
      Assert.That(member.LastName, Is.EqualTo(_expectedMemberResponse.LastName));
    }

}