using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.User.Register;
public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public RegisterUserTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response= await _httpClient.PostAsJsonAsync("User", request);

        response.StatusCode.ShouldBe<HttpStatusCode>(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        //acessa o documento, esse documento tem uma propriedade "name", pego como string o valor dessa propriedade
        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace(request.Name);
    }
}
