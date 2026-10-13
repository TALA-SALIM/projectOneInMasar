using Microsoft.AspNetCore.Http;

namespace homework.Services
{
    public interface IFileService
    {
        Task<byte[]> GetFile(string folderName, string fileName);
        Task<string> GetFileBase64(string folderName, string fileName);
        Task<string?> SaveFile(IFormFile? file, string folderName);
        Task<string?> SaveFile(byte[]? file, string folderName, string extension);
        Task<string?> SaveFile(string? file, string folderName, string extension);
        Task DeleteFile(string folderName, string? fileName);
    }
}