using Carter;

namespace Api.Modules;

public class HealthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new { status = "ok", version = "0.1.0" }));
        app.MapGet("/health/ready", () => Results.Ok(new { status = "ready", version = "0.1.0" }));
    }
}
