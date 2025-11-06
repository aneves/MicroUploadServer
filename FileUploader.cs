using Microsoft.AspNetCore.Mvc;

namespace MicroUploadServer;

public class FileUploader
{
    public static async Task<IResult> Upload([FromForm] IFormFile file)
    {
        if (file is null)
        {
            return TypedResults.BadRequest("No file sent.");
        }
        return TypedResults.Ok($"You sent {file.Length} bytes: `{file.FileName}`");
    }

    public static async Task<IResult> UploadMany([FromForm] IFormFileCollection files)
    {
        if (files is null || files.Count == 0)
        {
            return TypedResults.BadRequest("No files sent.");
        }

        return TypedResults.Ok($"You sent {files.Count} files. " + string.Join(" and ", files.Select(f => $"{f.Length}:`{f.FileName}`")));
    }
}
