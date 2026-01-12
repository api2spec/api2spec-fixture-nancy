using Nancy;
using Nancy.ModelBinding;

namespace Api.Modules;

public record Post(int Id, int UserId, string Title, string Body);

public class PostModule : NancyModule
{
    public PostModule() : base("/posts")
    {
        Get("/", _ => Response.AsJson(new[] {
            new Post(1, 1, "First Post", "Hello world"),
            new Post(2, 1, "Second Post", "Another post")
        }));

        Get("/{id:int}", args => Response.AsJson(new Post((int)args.id, 1, "Sample Post", "Post body")));

        Post("/", _ => {
            var post = this.Bind<Post>();
            return Response.AsJson(post with { Id = 1 }).WithStatusCode(HttpStatusCode.Created);
        });
    }
}
