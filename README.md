
# Micro-server for uploading files

This is a barebones server for uploading files.

## Running locally

To run locally:

1. Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
1. To be able to run locally in DEV mode, you may want or need to install the dev certs
    1. dotnet dev-certs https --trust
    1. For more information, read ["Trust the ASP.NET Core HTTPS development certificate" in learn.microsoft.com](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?#trust-the-aspnet-core-https-development-certificate)
1. Run the server with `dotnet run` from the terminal.

You can then access the server from your browser.  
The console will show you on what port it is running:
> Now listening on: http://localhost:5211
