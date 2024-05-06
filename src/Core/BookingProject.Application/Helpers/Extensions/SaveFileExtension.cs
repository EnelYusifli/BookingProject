using BookingProject.Application.CustomExceptions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookingProject.Application.Helpers.Extensions
{
    public static class SaveFileExtension
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task<string> SaveFile(this IFormFile image, string folderName)
        {
            if (image == null || image.Length == 0)
                throw new BadRequestException("File is empty.");

            if (image.Length > 2097152)
                throw new BadRequestException("File size exceeds the maximum allowed size.");

            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("Folder name cannot be null or empty.", nameof(folderName));

            if (_configuration == null)
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize before using SaveFile.");

            string apiKey = _configuration["GoogleCloud:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new NotFoundException("Google Cloud API key is missing or invalid.");

            var credential = GoogleCredential.FromFile(apiKey);
            var client = StorageClient.Create(credential);

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var objectName = $"{folderName}/{Guid.NewGuid()}_{image.FileName}";
                var bucketName = "bookingproject";

                await client.UploadObjectAsync(bucketName, objectName, null, memoryStream);
                return $"https://storage.googleapis.com/{bucketName}/{folderName}/{objectName}";
            }
        }
    }
}
