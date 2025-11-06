
# Micro-server for uploading files

This is a barebones server for uploading files.

Run locally with `dotnet watch`

Explore it by opening the page `/swagger`, such as https://localhost:5211/swagger  
Read the section "Exploring Swagger" below for more information.

## Running locally

To run locally:

1. Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
2. To be able to run locally in DEV mode, you may want or need to install the dev certs
    1. `dotnet dev-certs https --trust`
    2. For more information, read ["Trust the ASP.NET Core HTTPS development certificate" in learn.microsoft.com](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?#trust-the-aspnet-core-https-development-certificate)
3. Run the server with `dotnet run` from the terminal.

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

You can read more about configuring Swagger in the documentation: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?#customize-api-documentation

#### Known problem: multiple file uploads

Swagger does not seem to handle multiple-files correctly.  
It is not valid for testing the endpoint that uploads multiple files.

#### Exploring Swagger

You can use this live documentation to test the API. Example:

1. Press the row `[POST] /upload` to expand it.
2. See the extensive documentation — what the endpoint expects to receive, the known responses, etc.
3. Press the `[Try it out]` button on the right, just under the main header of this endpoint.
4. Fill in the values to send, and press `[Execute]`.
5. See the new content that appears, showing the server's response and other information.

In particular, the `curl` command allows you to replicate the request from a command line.  
