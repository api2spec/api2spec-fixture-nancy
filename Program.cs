using Nancy.Owin;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseOwin(x => x.UseNancy());
app.Run("http://0.0.0.0:8080");
