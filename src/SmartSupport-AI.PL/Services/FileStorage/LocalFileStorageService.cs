using SmartSupport_AI.BLL.Services.Interfaces.Conversations;

namespace SmartSupport_AI.PL.Services.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _uploadDirectory;

        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _uploadDirectory = Path.Combine(environment.ContentRootPath,"App_Data","Uploads");
        }

        public async Task<string> SaveFileAsync(Stream fileStream,string fileName,string contentType)
        {
            Directory.CreateDirectory(_uploadDirectory);

            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(_uploadDirectory,uniqueFileName);

            await using var fileStreamOutput = new FileStream(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write);

            await fileStream.CopyToAsync(fileStreamOutput);

            return Path.Combine("Uploads", uniqueFileName);
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.",nameof(filePath));

            var fullPath = Path.GetFullPath(
                Path.Combine(
                    _uploadDirectory,
                    filePath.Replace(
                        "Uploads" + Path.DirectorySeparatorChar,
                        string.Empty)));

            var uploadRoot = Path.GetFullPath(_uploadDirectory)
                + Path.DirectorySeparatorChar;

            if (!fullPath.StartsWith(
                    uploadRoot,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Invalid file path.");
            }

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
