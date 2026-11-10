using System.ComponentModel.DataAnnotations;

namespace BlogWebApi.Models;

public class BlogModel
{
    public int BlogId { get; set; }
    public string? BlogTitle { get; set; }
    public string? BlogAuthor { get; set; }
    public string? BlogContent { get; set; }
}

public class BlogRequestModel
{
    [Required(ErrorMessage = "Blog title is required.")]
    [StringLength(200, MinimumLength = 3)]
    public string? BlogTitle { get; set; }

    [Required(ErrorMessage = "Blog author is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string? BlogAuthor { get; set; }

    [Required(ErrorMessage = "Blog content is required.")]
    [MinLength(10, ErrorMessage = "Blog content must be at least 10 characters.")]
    public string? BlogContent { get; set; }
}

public class BlogResponseModel
{
    public int BlogId { get; set; }
    public string? BlogTitle { get; set; }
    public string? BlogAuthor { get; set; }
    public string? BlogContent { get; set; }
}