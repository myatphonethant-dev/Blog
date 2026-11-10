using BlogWebApi.Models;
using BlogWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly BlogService _blogService;

    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _blogService.GetAllBlog();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var result = await _blogService.GetBlogById(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromBody] BlogRequestModel request)
    {
        var result = await _blogService.CreateBlog(request);
        return CreatedAtAction(nameof(GetBlog), new { id = result.BlogId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogRequestModel request)
    {
        var result = await _blogService.UpdateBlog(id, request);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        var result = await _blogService.DeleteBlog(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBlogs([FromQuery] string query)
    {
        var result = await _blogService.SearchBlog(query);
        return Ok(result);
    }

    [HttpGet("author/{author}")]
    public async Task<IActionResult> GetBlogsByAuthor(string author)
    {
        var result = await _blogService.GetBlogByAuthor(author);
        return Ok(result);
    }

    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetBlogsByTitle(string title)
    {
        var result = await _blogService.GetBlogByTitle(title);
        return Ok(result);
    }
}