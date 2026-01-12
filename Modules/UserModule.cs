using Nancy;
using Nancy.ModelBinding;

namespace Api.Modules;

public record User(int Id, string Name, string Email);

public class UserModule : NancyModule
{
    public UserModule() : base("/users")
    {
        Get("/", _ => Response.AsJson(new[] {
            new User(1, "Alice", "alice@example.com"),
            new User(2, "Bob", "bob@example.com")
        }));

        Get("/{id:int}", args => Response.AsJson(new User((int)args.id, "Sample User", "user@example.com")));

        Post("/", _ => {
            var user = this.Bind<User>();
            return Response.AsJson(user with { Id = 1 }).WithStatusCode(HttpStatusCode.Created);
        });

        Put("/{id:int}", args => {
            var user = this.Bind<User>();
            return Response.AsJson(user with { Id = (int)args.id });
        });

        Delete("/{id:int}", _ => HttpStatusCode.NoContent);

        Get("/{userId:int}/posts", args => Response.AsJson(new[] {
            new { id = 1, userId = (int)args.userId, title = "User Post", body = "Content" }
        }));
    }
}
