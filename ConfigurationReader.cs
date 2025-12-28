namespace MicroUploadServer;

// To set the configuration:
// - Find the file "config.sample.json" in the executing folder.
// - Change its name to "config.json".
// - Edit the settings you want to change.
// - And restart this application.
public record class Config
{
    // A proper location could be something like: "~/Library/Application Support/MicroUploadServer/UploadedFiles"
    public string InboxFolder { get; set; } = "~/Desktop/UploadedFiles";
}

public static class ConfigurationReader
{
    public static Config ReadConfig()
    {
        IConfigurationRoot built = new ConfigurationBuilder()
            .AddJsonFile("config.json", optional: true)
            // Settings can be configured with env vars.
            // Example: set "UPLOADSERVER__InboxFolder" to "D:/UploadedFiles".
            // It should be case insensitive, but that has not been tested.
            .AddEnvironmentVariables(prefix: "uploadserver")
            .Build();
        Config config = built.Get<Config>()
            // Default values, if nothing is configured.
            ?? new();

        return config with
        {
            // Replace "~/foo" to "/users/johndoe/foo".
            InboxFolder = ReplaceUnixHomeDir(config.InboxFolder),
        };
    }

    /// <summary>
    /// Checks if a path is a "~/..." unix-path and converts it to an absolute path if it is.
    /// </summary>
    /// <remarks>
    /// This is needed because in C#,
    /// "~/foo" is not "/home/users/johndoe/foo"
    /// but something like "app/foo".
    /// </remarks>
    public static string ReplaceUnixHomeDir(string path)
    {
        if (!path.StartsWith('~'))
        {
            return path;
        }


        string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        switch (path)
        {
            case "~":
                return userFolder;
            default:
                return Path.Combine(
                    userFolder,
                    // Substring from the third character to the end: remove "~/" from the start.
                    path[2..]
                    );
        }
    }
}
