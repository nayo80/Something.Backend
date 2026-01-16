// using Microsoft.AspNetCore.Http;
// using Shared.Exceptions;
//
// namespace Shared.Helpers;
//
// public static class UploadImageHelper
// {
//     private static readonly string[] AllowedExtensions = [".jpeg",".jpg", ".png"];
//     private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
//
//     public static List<string> UploadFiles(IEnumerable<IFormFile> files)
//     {
//         if (files == null || !files.Any())
//         {
//             throw new UserFriendlyException(ErrorMessages.ImageNotUploaded);
//         }
//
//         // ბექგრაუნდში შენახვის ფოლდერი (პროექტის ცხრილთან მიმართებაში)
//         string baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "RestaurantImages");
//
//         if (!Directory.Exists(baseFolder))
//         {
//             Directory.CreateDirectory(baseFolder);
//         }
//         
//
//         var savedFilePaths = new List<string>();
//
//         foreach (var file in files)
//         {
//             if (file.Length == 0)
//             {
//                 throw new UserFriendlyException(ErrorMessages.ImageNotUploaded);
//             }
//
//             if (file.Length > MaxFileSize)
//             {
//                 throw new UserFriendlyException(ErrorMessages.ImageSizeIsMoreThan5Mb);
//             }
//
//             var fileExtension = Path.GetExtension(file.FileName).ToLower();
//
//             if (!AllowedExtensions.Contains(fileExtension))
//             {
//                 throw new UserFriendlyException(ErrorMessages.WrongImageExtension);
//             }
//
//             // შექმენი უნიკალური სახელი, რომ ფაილი არ გადაიწეროს
//             var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
//
//             var destinationFilePath = Path.Combine(baseFolder, uniqueFileName);
//
//             try
//             {
//                 using var stream = new FileStream(destinationFilePath, FileMode.Create);
//                 file.CopyTo(stream);
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception($"Error while uploading the file: {ex.Message}");
//             }
//
//             // შენახული ფაილის მისამართი (შეგიძლია აბრუნო აბსოლუტური ან რელატიური გზა)
//             savedFilePaths.Add(uniqueFileName);
//         }
//
//         return savedFilePaths;
//     }
// }
