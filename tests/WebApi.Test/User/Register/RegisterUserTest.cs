using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;
public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string method = "user";
    
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


    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await _httpClient.PostAsJsonAsync(method, request);

        response.StatusCode.ShouldBe<HttpStatusCode>(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await _httpClient.PostAsJsonAsync(method, request);

        response.StatusCode.ShouldBe<HttpStatusCode>(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_INVALID", new CultureInfo(culture));

        errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Registered(string culture)
    {
        // Arrange: Register a user with a specific email
        var request = RequestRegisterUserJsonBuilder.Build();

        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        // First registration should succeed
        var firstResponse = await _httpClient.PostAsJsonAsync("User", request);
        firstResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Act: Try to register again with the same email
        var duplicateRequest = RequestRegisterUserJsonBuilder.Build();
        duplicateRequest.Email = request.Email;

        var response = await _httpClient.PostAsJsonAsync("User", duplicateRequest);

        // Assert: Should return BadRequest with the correct error message
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));
        errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
    }
}
