using Microsoft.AspNetCore.Mvc;

namespace MicroUploadServer;

// TODO:
// - This does not handle file collisions.
// We have to figure out how to organize the files, and if we allow collisions, and how we handle collisions.
// We might need to receive more information from the sender, and place the file in a folder tree according to that information.
public class FileUploader(Configuration config, ILogger<FileUploader> logger)
{
    /// <summary>
    /// Initializes the prerequisites for the File Uploader to work.
    /// </summary>
    public void Initialize()
    {
        // Ensure folder exists.
        Directory.CreateDirectory(config.InboxFolder);
        logger.LogDebug("Files will be saved in folder '{Folder}'.", config.InboxFolder);
    }

    private bool FileNameSeemsSafe(string fileName)
    {
        if (fileName.Length == 0)
        {
            logger.LogInformation("Empty file name, rejecting.");
            return false;
        }

        // Not extensive.
        // This just tries to avoid path-traversal attacks.
        if (fileName.StartsWith("./"))
        {
            logger.LogWarning("File name starts with \"./\", rejecting to avoid path-traversal: {FileName}", fileName);
            return false;
        }
        if (fileName.Contains(".."))
        {
            // This may catch non-problematic things like "logged-data..xml".
            // It's a false-positive we accept to make the logic easier.
            logger.LogWarning("File name contains \"..\", rejecting to avoid path-traversal: '{FileName}'.", fileName);
            return false;
        }

        return true;
    }

    public async Task<IResult> Upload([FromForm] IFormFile file)
    {
        if (file.Length == 0)
        {
            return TypedResults.BadRequest($"File is empty.");
        }

        if (!FileNameSeemsSafe(file.FileName))
        {
            logger.LogWarning("File name seems invalid: '{FileName}'.", file.FileName);
            return TypedResults.BadRequest($"File name is invalid.");
        }

        string targetFullPath = Path.Combine(config.InboxFolder, file.FileName);
        if (File.Exists(targetFullPath))
        {
            logger.LogInformation("File already exists: '{FileName}'.", file.FileName);
            return TypedResults.BadRequest("File already exists.");
        }
        using (Stream fileStream = new FileStream(targetFullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        Helpers.LogFilesSaved(logger, [file]);
        return TypedResults.Ok("File saved.");
    }

    // TODO: check if we received two files with the same name.

    private record class FileWithPath(IFormFile File, string TargetFullPath);
    public async Task<IResult> UploadMany([FromForm] IFormFileCollection files)
    {
        if (files.Count == 0)
        {
            return TypedResults.BadRequest("No files sent.");
        }

        // Generate the paths to save, and pre-validate all files.
        // We want to validate ALL files before saving any, so we don't do a partial save.
        List<FileWithPath> filesWithPath = new(files.Count);
        int index = -1;
        foreach (IFormFile file in files)
        {
            index++;
            if (file.Length == 0)
            {
                return TypedResults.BadRequest($"File is empty at index {index}.");
            }

            if (!FileNameSeemsSafe(file.FileName))
            {
                logger.LogWarning("File name seems invalid for file at index {Index}: '{FileName}'.", index, file.FileName);
                return TypedResults.BadRequest($"File name is invalid for file at index {index}.");
            }

            string filePath = Path.Combine(config.InboxFolder, file.FileName);
            if (File.Exists(filePath))
            {
                logger.LogInformation("File already exists: file index {Index} named '{FileName}'.", index, file.FileName);
                return TypedResults.BadRequest($"File already exists for file at index {index}.");
            }
            filesWithPath.Add(new FileWithPath(file, filePath));
        }
        foreach (FileWithPath item in filesWithPath)
        {
            using Stream fileStream = new FileStream(item.TargetFullPath, FileMode.Create);
            await item.File.CopyToAsync(fileStream);
        }

        Helpers.LogFilesSaved(logger, [.. files]);
        return TypedResults.Ok($"{filesWithPath.Count} files saved.");
    }

    private static class Helpers
    {
        public static void LogFilesSaved(ILogger<FileUploader> logger, IFormFile[] files)
        {
            logger.LogInformation("Saved {Count} files.", files.Length);
            logger.LogDebug(
                "Saved {Count} files for a total of {TotalSize}. Files: {FileDescriptions}.",
                files.Length,
                HumanizeFileSize(files.Sum(file => file.Length)),
                string.Join(
                    " and ",
                    files
                        .Select(f => $"{HumanizeFileSize(f.Length)}:'{f.FileName}'")
                    )
                );
        }

        private static readonly string[] sizes = ["B", "kB", "MB"];
        public static string HumanizeFileSize(long fileSizeInBytes)
        {
            double size = fileSizeInBytes;
            int order = 0;
            while (size >= 1000 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            // Include one decimal case if the number is small...
            if (size < 10)
            {
                // "2.5 MB" — single decimal place, NBSP between value and units.
                return string.Format("{0:0.0} {1}", size, sizes[order]);
            }
            // ... otherwise just show the integer part.
            // "583 kB" — integer, NBSP between value and units.
            return string.Format("{0:0} {1}", size, sizes[order]);
        }
    }
}
