using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests;

public class UserModuleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserModuleTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsOkWithUserList()
    {
        // Act
        var response = await _client.GetAsync("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<User[]>();
        users.Should().NotBeNull();
        users!.Length.Should().Be(2);
        users[0].Name.Should().Be("Alice");
        users[1].Name.Should().Be("Bob");
    }

    [Fact]
    public async Task GetUserById_ReturnsOkWithUser()
    {
        // Act
        var response = await _client.GetAsync("/users/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<User>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(1);
        user.Name.Should().Be("Sample User");
    }

    [Fact]
    public async Task CreateUser_WithValidData_Returns201Created()
    {
        // Arrange
        var newUser = new User(0, "Charlie", "charlie@example.com");

        // Act
        var response = await _client.PostAsJsonAsync("/users", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var user = await response.Content.ReadFromJsonAsync<User>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(1);
        user.Name.Should().Be("Charlie");
    }

    [Fact]
    public async Task UpdateUser_ReturnsOkWithUpdatedUser()
    {
        // Arrange
        var updatedUser = new User(0, "Alice Updated", "alice.updated@example.com");

        // Act
        var response = await _client.PutAsJsonAsync("/users/1", updatedUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<User>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(1);
        user.Name.Should().Be("Alice Updated");
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent()
    {
        // Act
        var response = await _client.DeleteAsync("/users/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsOkWithPosts()
    {
        // Act
        var response = await _client.GetAsync("/users/1/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var posts = await response.Content.ReadFromJsonAsync<UserPost[]>();
        posts.Should().NotBeNull();
        posts!.Length.Should().Be(1);
        posts[0].UserId.Should().Be(1);
        posts[0].Title.Should().Be("User Post");
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ReturnsNotFound()
    {
        // Act - testing that non-matching route pattern returns 404
        var response = await _client.GetAsync("/users/invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserById_WithDifferentId_ReturnsCorrectId()
    {
        // Act
        var response = await _client.GetAsync("/users/42");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<User>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(42);
    }

    [Fact]
    public async Task CreateUser_WithEmptyBody_Returns400BadRequest()
    {
        // Arrange
        var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/users", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUserById_WithIdOver100_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/users/101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_WithIdOver100_ReturnsNotFound()
    {
        // Arrange
        var user = new User(0, "Test User", "test@example.com");

        // Act
        var response = await _client.PutAsJsonAsync("/users/101", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_WithIdOver100_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/users/101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserPosts_WithUserIdOver100_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/users/101/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private record User(int Id, string Name, string Email);
    private record UserPost(int Id, int UserId, string Title, string Body);
}
