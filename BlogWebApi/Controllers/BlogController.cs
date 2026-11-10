using BlogWebApi.Models;
using BlogWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController(BlogService blogService) : ControllerBase
{
    private readonly BlogService _blogService = blogService;

    [HttpGet("List")]
    public async Task<IActionResult> GetList()
    {
        var result = await _blogService.GetAllBlog();

        return Ok(result);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var result = await _blogService.GetBlogById(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateBlog([FromBody] BlogRequestModel request)
    {
        var result = await _blogService.CreateBlog(request);

        return CreatedAtAction(
            nameof(GetBlog),
            new { id = result.Data!.BlogId },
            result
        );
    }

    [HttpPut("Update/{id:int}")]
    public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogRequestModel request)
    {
        var result = await _blogService.UpdateBlog(id, request);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        var result = await _blogService.DeleteBlog(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> SearchBlog([FromQuery] string query)
    {
        var result = await _blogService.SearchBlog(query);
        return Ok(result);
    }

    [HttpGet("Author/{author}")]
    public async Task<IActionResult> GetBlogsByAuthor(string author)
    {
        var result = await _blogService.GetBlogByAuthor(author);
        return Ok(result);
    }

    [HttpGet("Title/{title}")]
    public async Task<IActionResult> GetBlogsByTitle(string title)
    {
        var result = await _blogService.GetBlogByTitle(title);
        return Ok(result);
    }
}