namespace BlogWebApi.AppDbContext;

public class Blog
{
    public int Id { get; set; }
    public string? BlogTitle { get; set; }
    public string? BlogAuthor { get; set; }
    public string? BlogContent { get; set; }
}