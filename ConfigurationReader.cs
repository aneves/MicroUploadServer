namespace MicroUploadServer;

public static class ConfigurationReader
{
    public static Configuration ReadConfig()
    {
        IConfigurationRoot built = new ConfigurationBuilder()
            .AddJsonFile("config.json", optional: true)
            // Settings can be configured with env vars.
            // Example: set "UPLOADSERVER__InboxFolder" to "D:/UploadedFiles".
            // It should be case insensitive, but that has not been tested.
            .AddEnvironmentVariables(prefix: "uploadserver")
            .Build();
        Configuration config = built.Get<Configuration>()
            // Default values, if nothing is configured.
            ?? new();

        return config with
        {
            // Replace "~/foo" to "/users/alice/foo".
            InboxFolder = ReplaceUnixHomeDir(config.InboxFolder),
        };
    }

    /// <summary>
    /// Checks if a path is a "~/..." unix-path and converts it to an absolute path if it is.
    /// </summary>
    /// <remarks>
    /// This is needed because in C#,
    /// "~/foo" is not "/home/users/alice/foo"
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
            // Special handling of "~/", to keep the trailing slash.
            // This keeps that result consistent with the other cases that also keep trailing slashes, like "~/foo/".
            case string _
                    when path.Length == 2
                        && (path[1] == Path.DirectorySeparatorChar
                            || path[1] == Path.AltDirectorySeparatorChar):
                return userFolder + Path.DirectorySeparatorChar;
            default:
                return Path.Combine(
                    userFolder,
                    // Substring from the third character to the end: remove "~/" from the start.
                    path[2..]
                    );
        }
    }
}
