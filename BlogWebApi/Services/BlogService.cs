using BlogWebApi.AppDbContext;
using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.Services;

public class BlogService(EfAppDbContext db)
{
    private readonly EfAppDbContext _db = db;

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

    public async Task<ApiResponse<BlogModel>> CreateBlog(BlogRequestModel request)
    {
        var blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogAuthor = request.BlogAuthor,
            BlogContent = request.BlogContent
        };

        _db.Blogs.Add(blog);
        await _db.SaveChangesAsync();

        var blogModel = new BlogModel
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };

        return new ApiResponse<BlogModel>
        {
            Success = true,
            Message = "Blog created successfully.",
            Data = blogModel
        };
    }

    public async Task<ApiResponse<BlogModel>> UpdateBlog(int id, BlogRequestModel request)
    {
        var blog = await _db.Blogs.FindAsync(id);

        if (blog == null)
        {
            return new ApiResponse<BlogModel>
            {
                Success = false,
                Message = "Blog not found."
            };
        }

        blog.BlogTitle = request.BlogTitle;
        blog.BlogAuthor = request.BlogAuthor;
        blog.BlogContent = request.BlogContent;

        await _db.SaveChangesAsync();

        return new ApiResponse<BlogModel>
        {
            Success = true,
            Message = "Blog updated successfully.",
            Data = new BlogModel
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogAuthor = blog.BlogAuthor,
                BlogContent = blog.BlogContent
            }
        };
    }

    public async Task<ApiResponse<BlogModel>> DeleteBlog(int id)
    {
        var blog = await _db.Blogs.FindAsync(id);

        if (blog == null)
        {
            return new ApiResponse<BlogModel>
            {
                Success = false,
                Message = "Blog not found."
            };
        }

        _db.Blogs.Remove(blog);
        await _db.SaveChangesAsync();

        return new ApiResponse<BlogModel>
        {
            Success = true,
            Message = "Blog deleted successfully."
        };
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
            .Where(b => b.BlogAuthor!.Contains(author.ToLower().Trim()))
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
            .Where(b => b.BlogTitle!.Contains(title.ToLower().Trim()))
            .Select(b => new BlogModel
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogAuthor = b.BlogAuthor,
                BlogContent = b.BlogContent
            }).ToListAsync();
    }
}