using Carter;

namespace Api.Modules;

public record Post(int Id, int UserId, string Title, string Body);

public class PostModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts", () => Results.Ok(new[] {
            new Post(1, 1, "First Post", "Hello world"),
            new Post(2, 1, "Second Post", "Another post")
        }));

        app.MapGet("/posts/{id:int}", (int id) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.Ok(new Post(id, 1, "Sample Post", "Post body"));
        });

        app.MapPost("/posts", (Post post) => Results.Created($"/posts/{1}", post with { Id = 1 }));

        app.MapPut("/posts/{id:int}", (int id, Post post) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.Ok(post with { Id = id });
        });

        app.MapDelete("/posts/{id:int}", (int id) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.NoContent();
        });
    }
}
