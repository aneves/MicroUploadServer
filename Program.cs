using MicroUploadServer;

var builder = WebApplication.CreateBuilder(args);

// Configure Swagger support.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Micro-Upload Server";
    config.Title = "Micro-Upload Server v1";
    config.Version = "v1";
});

/*
TODO: configure max RequestBodysize.
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});
 
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
*/

var app = builder.Build();

// Add Swagger in development.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Micro-Upload Server";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.ConfigureRoutes();

app.Run();
