using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.AppDbContext;

[Keyless]
[Table("Blog")]
public partial class Blog
{
    public int BlogId { get; set; }

    [Unicode(false)]
    public string? BlogTitle { get; set; }

    [Unicode(false)]
    public string? BlogAuthor { get; set; }

    [Unicode(false)]
    public string? BlogContent { get; set; }
}
