using BlogWebApi.AppDbContext;
using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.Services;

public class BlogService
{
    private readonly EfAppDbContext _db;

    public BlogService(EfAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<BlogModel>> GetAllBlog()
    {
        return await _db.Blogs.Select(b => new BlogModel
        {
            BlogId = b.BlogId,
            BlogTitle = b.BlogTitle,
            BlogAuthor = b.BlogAuthor,
            BlogContent = b.BlogContent
        }).ToListAsync();
    }

    public async Task<BlogModel?> GetBlogById(int id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return null;
        return new BlogModel
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }

    public async Task<BlogModel> CreateBlog(BlogRequestModel request)
    {
        var blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogAuthor = request.BlogAuthor,
            BlogContent = request.BlogContent
        };
        _db.Blogs.Add(blog);
        await _db.SaveChangesAsync();
        return new BlogModel
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }

    public async Task<bool> UpdateBlog(int id, BlogRequestModel request)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return false;
        blog.BlogTitle = request.BlogTitle;
        blog.BlogAuthor = request.BlogAuthor;
        blog.BlogContent = request.BlogContent;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBlog(int id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return false;
        _db.Blogs.Remove(blog);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<BlogModel>> SearchBlog(string searchTerm)
    {
        return await _db.Blogs
            .Where(b => b.BlogTitle!.Contains(searchTerm) || b.BlogAuthor!.Contains(searchTerm))
            .Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }

    public async Task<List<BlogModel>> GetBlogByAuthor(string author)
    {
        return await _db.Blogs
            .Where(b => b.BlogAuthor == author)
            .Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }

    public async Task<List<BlogModel>> GetBlogByTitle(string title)
    {
        return await _db.Blogs
            .Where(b => b.BlogTitle == title)
            .Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }
}