namespace MicroUploadServer;

public static class Routes
{
    public static void ConfigureRoutes(this WebApplication app, Configuration config)
    {
        var logger = app.Services.GetRequiredService<ILogger<FileUploader>>();
        FileUploader uploader = new(config, logger);

        uploader.Initialize();

        // TODO: enable and implement anti-forgery.
        // See SO: https://stackoverflow.com/questions/77189996/upload-files-to-a-minimal-api-endpoint-in-net-8/77191406#77191406
        // Anti-forgery is required to use [FromForm] for file uploads.
        app.MapPost("/upload", uploader.Upload)
            .DisableAntiforgery()
            ;
        app.MapPost("/multi-upload", uploader.UploadMany)
            .DisableAntiforgery()
            ;
    }
}
