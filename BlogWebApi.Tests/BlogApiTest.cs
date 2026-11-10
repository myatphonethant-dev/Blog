
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BlogWebApi.Tests;

public class BlogApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BlogApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static object CreateBlogRequest(
        string title = "Integration Test Blog",
        string author = "Test Author",
        string content = "This is test blog content.")
    {
        return new
        {
            BlogTitle = title,
            BlogAuthor = author,
            BlogContent = content
        };
    }

    private async Task<int> CreateTestBlogAsync()
    {
        var request = CreateBlogRequest(
            $"Test Blog {Guid.NewGuid()}");

        var response = await _client.PostAsJsonAsync("/api/Blog", request);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(json.TryGetProperty("blogId", out var id)
                    || json.TryGetProperty("BlogId", out id),
                    "POST response must contain BlogId.");

        return id.GetInt32();
    }

    // 1. GET ALL BLOGS
    [Fact]
    public async Task GetBlogs_ShouldReturnSuccessAndListOfBlogs()
    {
        var response = await _client.GetAsync("/api/Blog");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(content);

        Assert.Equal(
            JsonValueKind.Array,
            document.RootElement.ValueKind);
    }

    // 2. GET BLOG BY ID
    [Fact]
    public async Task GetBlogById_ShouldReturnBlog_WhenBlogExists()
    {
        var blogId = await CreateTestBlogAsync();

        var response = await _client.GetAsync($"/api/Blog/{blogId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var blog = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            blogId,
            blog.GetProperty("blogId").GetInt32());
    }

    // 3. GET BLOG BY ID - NOT FOUND
    [Fact]
    public async Task GetBlogById_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        var response = await _client.GetAsync("/api/Blog/999999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 4. CREATE BLOG
    [Fact]
    public async Task CreateBlog_ShouldCreateBlogSuccessfully()
    {
        var request = CreateBlogRequest(
            $"New Blog {Guid.NewGuid()}");

        var response = await _client.PostAsJsonAsync("/api/Blog", request);

        Assert.True(
            response.StatusCode == HttpStatusCode.Created ||
            response.StatusCode == HttpStatusCode.OK);

        var blog = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(
            blog.TryGetProperty("blogId", out _) ||
            blog.TryGetProperty("BlogId", out _));
    }

    // 5. UPDATE BLOG
    [Fact]
    public async Task UpdateBlog_ShouldUpdateBlogSuccessfully()
    {
        var blogId = await CreateTestBlogAsync();

        var updateRequest = CreateBlogRequest(
            "Updated Blog Title",
            "Updated Author",
            "Updated blog content.");

        var response = await _client.PutAsJsonAsync(
            $"/api/Blog/{blogId}",
            updateRequest);

        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/Blog/{blogId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var updatedBlog =
            await getResponse.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            "Updated Blog Title",
            updatedBlog.GetProperty("blogTitle").GetString());
    }

    // 6. UPDATE BLOG - NOT FOUND
    [Fact]
    public async Task UpdateBlog_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        var request = CreateBlogRequest(
            "Updated Title",
            "Updated Author",
            "Updated Content");

        var response = await _client.PutAsJsonAsync(
            "/api/Blog/999999999",
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 7. DELETE BLOG
    [Fact]
    public async Task DeleteBlog_ShouldDeleteBlogSuccessfully()
    {
        var blogId = await CreateTestBlogAsync();

        var response = await _client.DeleteAsync($"/api/Blog/{blogId}");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/Blog/{blogId}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    // 8. DELETE BLOG - NOT FOUND
    [Fact]
    public async Task DeleteBlog_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        var response = await _client.DeleteAsync("/api/Blog/999999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 9. CREATE BLOG - INVALID REQUEST
    [Fact]
    public async Task CreateBlog_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        var invalidRequest = new
        {
            BlogTitle = "",
            BlogAuthor = "",
            BlogContent = ""
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Blog",
            invalidRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
