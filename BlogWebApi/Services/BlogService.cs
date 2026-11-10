using BlogWebApi.Models;

namespace BlogWebApi.Services;

public class BlogService
{
    private readonly AppDbContext _db;

    public BlogService(AppDbContext db) => _db = db;

    public async Task<List<BlogModel>> GetAllBlogsAsync()
    {
        return await _db.Blogs.Select(b => new BlogModel
        {
            Id = b.Id,
            BlogTitle = b.BlogTitle,
            BlogAuthor = b.BlogAuthor,
            BlogContent = b.BlogContent
        }).ToListAsync();
    }

    public async Task<BlogModel?> GetBlogByIdAsync(int id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return null;
        return new BlogModel
        {
            Id = blog.Id,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }

    public async Task<BlogModel> CreateBlogAsync(BlogRequestModel request)
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
            Id = blog.Id,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }

    public async Task<bool> UpdateBlogAsync(int id, BlogRequestModel request)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return false;
        blog.BlogTitle = request.BlogTitle;
        blog.BlogAuthor = request.BlogAuthor;
        blog.BlogContent = request.BlogContent;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBlogAsync(int id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return false;
        _db.Blogs.Remove(blog);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<BlogModel>> SearchBlogsAsync(string searchTerm)
    {
        return await _db.Blogs
            .Where(b => b.BlogTitle!.Contains(searchTerm) || b.BlogAuthor!.Contains(searchTerm))
            .Select(b => new BlogModel
            {
                Id = b.Id,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }

    public async Task<List<BlogModel>> GetBlogsByAuthorAsync(string author)
    {
        return await _db.Blogs
            .Where(b => b.BlogAuthor == author)
            .Select(b => new BlogModel
            {
                Id = b.Id,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }

    public async Task<List<BlogModel>> GetBlogsByTitleAsync(string title)
    {
        return await _db.Blogs
            .Where(b => b.BlogTitle == title)
            .Select(b => new BlogModel
            {
                Id = b.Id,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    } 
}