
# Micro-server for uploading files

This is a barebones server for uploading files.

Run locally with `dotnet run` or by debugging from VS Code.

## Configuring

To configure the folder where the files are saved:

1. Copy the file "config.sample.json" to a new file named "config.json".
2. Edit the values.
3. And restart the application.

Check the code comments on [ConfigurationReader.cs](ConfigurationReader.cs) for more information about the properties.

## Running locally

To run locally:

1. Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
2. To be able to run locally in DEV mode, you may want/need to install the dev certs
    1. `dotnet dev-certs https --trust`
    2. For more information, read ["Trust the ASP.NET Core HTTPS development certificate" in learn.microsoft.com](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?#trust-the-aspnet-core-https-development-certificate)
3. Run the server
    1. From VS Code: press `F5`
    2. From Terminal: with `dotnet run`

You can then access the server from your browser at http://localhost:5211/swagger

The console will show you on what port it is running:
> info: Microsoft.Hosting.Lifetime[14]  
>       Now listening on: http://localhost:5211

## Developing

If you want to edit the code, running with `dotnet watch` is better.  
This enables "hot reload", and refreshes the server automatically when it detects any changes.

### API documentation and testing

This project has Swagger enabled.  
If running it in development, you can open the live documentation on `/swagger`: https://localhost:5211/swagger

In this page you get an overview of the available endpoints, and can even test them there.

You can read more about configuring Swagger in the documentation: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?#customize-api-documentation

#### Known problem: multiple file uploads

Swagger does not seem to handle multiple-files correctly.  
It shows the files input as strings instead of file inputs, when testing the endpoint that uploads multiple files.

#### Exploring Swagger

You can use this live documentation to test the API. Example:

1. Press the row `[POST] /upload` to expand it.
2. See the documentation — what the endpoint expects to receive, the known responses, etc.
3. Press the `[Try it out]` button on the right, just under the main header of this endpoint.
4. Pick a file to send, and press `[Execute]`.
5. See the new content that appears:
    1. The corresponding `curl` command.
    2. The server's response below: 200 "File saved.", or 400 Bad Request "File already exists.", or other responses.

In particular, the `curl` command allows you to do the same request from a command line.  
You can test the multiple-upload endpoint via command line using curl:

```bash
curl -X 'POST' \                                                                     
  'http://localhost:5211/multi-upload' \
  -H 'accept: */*' \
  -H 'Content-Type: multipart/form-data' \
  -F 'files=@sample1.txt' -F 'files=@sample2.txt'
```
