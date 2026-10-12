using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace homework.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> GetFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(
                _env.WebRootPath,
                folderName,
                Path.GetFileName(fileName));

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> GetFileBase64(string folderName, string fileName)
        {
            var file = await GetFile(folderName, fileName);
            return Convert.ToBase64String(file);
        }

        public async Task<string?> SaveFile(IFormFile? file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploads = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString("N")
                + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploads, fileName);

            await using var fileStream =
                new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return fileName;
        }

        public async Task<string?> SaveFile(
            byte[]? file,
            string folderName,
            string extension)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploads = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString("N") + extension;
            var filePath = Path.Combine(uploads, fileName);

            await File.WriteAllBytesAsync(filePath, file);

            return fileName;
        }

        public async Task<string?> SaveFile(
            string? file,
            string folderName,
            string extension)
        {
            if (string.IsNullOrWhiteSpace(file))
                return null;

            var commaIndex = file.IndexOf(",", StringComparison.Ordinal);
            if (commaIndex >= 0)
                file = file[(commaIndex + 1)..];

            var bytes = Convert.FromBase64String(file);
            var uploads = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString("N") + extension;
            var filePath = Path.Combine(uploads, fileName);

            await File.WriteAllBytesAsync(filePath, bytes);

            return fileName;
        }

        public Task DeleteFile(string folderName, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Task.CompletedTask;

            var filePath = Path.Combine(
                _env.WebRootPath,
                folderName,
                Path.GetFileName(fileName));

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}