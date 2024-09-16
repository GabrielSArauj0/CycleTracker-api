using Ciclo.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Ciclo.Application.Contracts;

public interface IFileService
{
    Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath, EPathAccess pathAccess = EPathAccess.Public, int urlLimitLength = 255);
    string ObterPath(Uri uri);
    bool Apagar(Uri uri);
}