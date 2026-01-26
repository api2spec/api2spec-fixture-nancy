using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests;

public class PostModuleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PostModuleTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPosts_ReturnsOkWithPostList()
    {
        // Act
        var response = await _client.GetAsync("/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var posts = await response.Content.ReadFromJsonAsync<Post[]>();
        posts.Should().NotBeNull();
        posts!.Length.Should().Be(2);
        posts[0].Title.Should().Be("First Post");
        posts[1].Title.Should().Be("Second Post");
    }

    [Fact]
    public async Task GetPostById_ReturnsOkWithPost()
    {
        // Act
        var response = await _client.GetAsync("/posts/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var post = await response.Content.ReadFromJsonAsync<Post>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(1);
        post.Title.Should().Be("Sample Post");
    }

    [Fact]
    public async Task CreatePost_WithValidData_Returns201Created()
    {
        // Arrange
        var newPost = new Post(0, 1, "New Post", "Post content");

        // Act
        var response = await _client.PostAsJsonAsync("/posts", newPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var post = await response.Content.ReadFromJsonAsync<Post>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(1);
        post.Title.Should().Be("New Post");
    }

    [Fact]
    public async Task GetPostById_WithInvalidId_ReturnsNotFound()
    {
        // Act - testing that non-matching route pattern returns 404
        var response = await _client.GetAsync("/posts/invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPostById_WithDifferentId_ReturnsCorrectId()
    {
        // Act
        var response = await _client.GetAsync("/posts/42");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var post = await response.Content.ReadFromJsonAsync<Post>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(42);
    }

    [Fact]
    public async Task CreatePost_WithEmptyBody_Returns400BadRequest()
    {
        // Arrange
        var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/posts", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdatePost_ReturnsOkWithUpdatedPost()
    {
        // Arrange
        var updatedPost = new Post(0, 1, "Updated Post", "Updated content");

        // Act
        var response = await _client.PutAsJsonAsync("/posts/1", updatedPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var post = await response.Content.ReadFromJsonAsync<Post>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(1);
        post.Title.Should().Be("Updated Post");
    }

    [Fact]
    public async Task DeletePost_ReturnsNoContent()
    {
        // Act
        var response = await _client.DeleteAsync("/posts/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetPostById_WithIdOver100_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/posts/101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePost_WithIdOver100_ReturnsNotFound()
    {
        // Arrange
        var post = new Post(0, 1, "Test Post", "Content");

        // Act
        var response = await _client.PutAsJsonAsync("/posts/101", post);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePost_WithIdOver100_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/posts/101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private record Post(int Id, int UserId, string Title, string Body);
}
