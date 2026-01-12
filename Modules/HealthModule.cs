using Nancy;

namespace Api.Modules;

public class HealthModule : NancyModule
{
    public HealthModule()
    {
        Get("/health", _ => Response.AsJson(new { status = "ok", version = "0.1.0" }));
        Get("/health/ready", _ => Response.AsJson(new { status = "ready", version = "0.1.0" }));
    }
}
