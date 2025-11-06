
# Micro-server for uploading files

This is a barebones server for uploading files.

## Running locally

To run locally:

1. Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
1. To be able to run locally in DEV mode, you may want or need to install the dev certs
    1. `dotnet dev-certs https --trust`
    1. For more information, read ["Trust the ASP.NET Core HTTPS development certificate" in learn.microsoft.com](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?#trust-the-aspnet-core-https-development-certificate)
1. Run the server with `dotnet run` from the terminal.

You can then access the server from your browser.  
The console will show you on what port it is running:
> info: Microsoft.Hosting.Lifetime[14]  
>       Now listening on: http://localhost:5211

## Developing

### Live changes

If you want to edit the code, running with `dotnet watch` instead will enable "hot reload", and restart the server automatically when it detects any changes.

### API documentation and testing

This project has Swagger enabled.  
If running it in development, you can open the live documentation on `/swagger`, such as https://localhost:5211/swagger

In this page you get an overview of the available endpoints, and can even test them there.  
Press one of the lines to expand them.

You can read more about configuring Swagger in the documentation: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?#customize-api-documentation
