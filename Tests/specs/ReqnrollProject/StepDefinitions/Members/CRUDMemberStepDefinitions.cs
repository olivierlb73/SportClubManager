using SportClubManager.Core.Application.Members.Commands.CreateMember;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity.Data;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;
using NuGet.Protocol;
using SportClubManager.Core.Application.Members.Queries.GetMemberById;
using SportClubManager.Core.Application.Members.Queries;
using Microsoft.AspNetCore.Mvc.Testing;
using SportClubManager.Core.Application.Members.Commands.UpdateMember;
using System.Net;

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
        CreateMemberRequest createMemberRequest = new (row["firstName"], row["lastName"], row["email"]);
            
        var result = await _httpClient.PostAsJsonAsync("/api/members", createMemberRequest);

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

    [Then("I can retrieve the list of members")]
    public async Task ICanRetrieveTheListOfMembers()
    {
      var membersResponse = await _httpClient.GetAsync($"/api/members");

      membersResponse.EnsureSuccessStatusCode();

      var members = await membersResponse.Content.ReadFromJsonAsync<IEnumerable<MemberResponse>>();

      Assert.That(members, Has.Count.GreaterThan(0));
    }
    
    [When("I update the new member with")]
    public async Task IUpdateTheNewMemberWith(Table table)
    {
        var row = table.Rows[0];
        UpdateMemberRequest updateMemberRequest = new (row["firstName"], row["lastName"], row["email"]);
            
        var result = await _httpClient.PutAsJsonAsync($"/api/members/{_memberId}", updateMemberRequest);

        result.EnsureSuccessStatusCode();
        
        _expectedMemberResponse = new MemberResponse(_memberId, row["firstName"], row["lastName"]);
    }

    [When("I delete the new member")]
    public async Task IDeleteTheNewMember()
    {
        var memberResponse = await _httpClient.DeleteAsync($"/api/members/{_memberId}");

        memberResponse.EnsureSuccessStatusCode();
    }

    [Then("I can not retrieve the new member")]
    public async Task ICanNotRetrieveTheNewMember()
    {
      var memberResponse = await _httpClient.GetAsync($"/api/members/{_memberId}");

      Assert.That(memberResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

}