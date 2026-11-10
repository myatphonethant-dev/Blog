```
dotnet ef dbcontext scaffold "Server=localhost;Database=BlogDb;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir AppDbContext --context EfAppDbContext --context-dir AppDbContext --data-annotations --force
```