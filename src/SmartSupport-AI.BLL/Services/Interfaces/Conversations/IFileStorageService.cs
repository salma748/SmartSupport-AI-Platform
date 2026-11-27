using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Interfaces.Conversations
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(
            Stream fileStream,
            string fileName,
            string contentType);

        Task DeleteFileAsync(string filePath);
    }
}
