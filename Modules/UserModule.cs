using Carter;

namespace Api.Modules;

public record User(int Id, string Name, string Email);

public class UserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", () => Results.Ok(new[] {
            new User(1, "Alice", "alice@example.com"),
            new User(2, "Bob", "bob@example.com")
        }));

        app.MapGet("/users/{id:int}", (int id) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.Ok(new User(id, "Sample User", "user@example.com"));
        });

        app.MapPost("/users", (User user) => Results.Created($"/users/{1}", user with { Id = 1 }));

        app.MapPut("/users/{id:int}", (int id, User user) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.Ok(user with { Id = id });
        });

        app.MapDelete("/users/{id:int}", (int id) =>
        {
            if (id > 100) return Results.NotFound();
            return Results.NoContent();
        });

        app.MapGet("/users/{userId:int}/posts", (int userId) =>
        {
            if (userId > 100) return Results.NotFound();
            return Results.Ok(new[] {
                new { id = 1, userId = userId, title = "User Post", body = "Content" }
            });
        });
    }
}
