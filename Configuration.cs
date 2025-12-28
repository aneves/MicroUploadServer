namespace MicroUploadServer;

/// <summary>
/// The configuration for the Server.
/// The README shows how to set this, in the section "Configuring".
/// </summary>
public record class Configuration
{
    /// <summary>
    /// The folder where the received files will be saved.
    /// Defaults for now to a folder on the Desktop.
    /// </summary>
    /// <remarks>
    /// Paths starting with tilde, like "~/foo",
    ///    will be converted to "/home/users/alice/foo" so they land under the user's folder.<br />
    /// Other paths will be used unchanged.
    /// </remarks>
    public string InboxFolder { get; set; } = "~/Desktop/UploadedFiles";

    // A proper location for the InboxFolder could be something like:
    // 1. Globally:
    //        Mac:          "/Library/Application Support/MicroUploadServer/UploadedFiles"
    //        Linux or Mac: "/var/lib/MicroUploadServer/UploadedFiles"
    //        Windows:      "\ProgramData\MicroUploadServer\UploadedFiles"
    // 2. Or under user data:
    //        Mac:                "~/Library/Application Support/MicroUploadServer/UploadedFiles"
    //        Linux or Mac: maybe "~/MicroUploadServer/UploadedFiles"
    //        Windows:            "~\AppData\Roaming\MicroUploadServer/UploadedFiles"
}
