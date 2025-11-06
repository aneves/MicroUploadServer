namespace MicroUploadServer;

public static class Routes
{
    public static void ConfigureRoutes(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
    }
}
