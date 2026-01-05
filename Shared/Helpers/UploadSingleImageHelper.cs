using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Shared.Helpers;

public static class UploadSingleImageHelper
{
    private static readonly string[] AllowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    public static string UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new UserFriendlyException(ErrorMessages.ImageNotUploaded);
        }

        if (file.Length > MaxFileSize)
        {
            throw new UserFriendlyException(ErrorMessages.ImageSizeIsMoreThan5Mb);
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!AllowedExtensions.Contains(fileExtension))
        {
            throw new UserFriendlyException(ErrorMessages.WrongImageExtension);
        }

        string baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "RestaurantImages");

        if (!Directory.Exists(baseFolder))
        {
            Directory.CreateDirectory(baseFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var destinationFilePath = Path.Combine(baseFolder, uniqueFileName);

        try
        {
            using var stream = new FileStream(destinationFilePath, FileMode.Create);
            file.CopyTo(stream);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while uploading the file: {ex.Message}");
        }

        return uniqueFileName; 
    }
}
