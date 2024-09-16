using Ciclo.Application.Contracts;
using Ciclo.Core.Enums;
using Ciclo.Core.Extensions;
using Ciclo.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ciclo.Application.Services;

public class FileService : IFileService
{
    private readonly AppSettings _appSettings;
    private readonly UploadSettings _uploadSettings;

    public FileService(IOptions<AppSettings> appSettings, IOptions<UploadSettings> uploadSettings)
    {
        _appSettings = appSettings.Value;
        _uploadSettings = uploadSettings.Value;
    }
    
    public async Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath, EPathAccess pathAccess = EPathAccess.Public, int urlLimitLength = 255)
    {
        var fileName = GenerateNewFileName(arquivo.FileName, pathAccess, uploadPath, urlLimitLength);
        var filePath = MountFilePath(fileName, uploadPath);

        try
        {
            await File.WriteAllBytesAsync(filePath, ConvertFileInByteArray(arquivo));
        }
        catch (DirectoryNotFoundException)
        {
            var file = new FileInfo(filePath);
            file.Directory?.Create();
            await File.WriteAllBytesAsync(filePath, ConvertFileInByteArray(arquivo));
        }

        return GetFileUrl(fileName, pathAccess, uploadPath);
    }

    public string ObterPath(Uri uri)
    {
        return GetFilePath(uri);
    }

    public bool Apagar(Uri uri)
    {
        try
        {
            var filePath = GetFilePath(uri);

            new FileInfo(filePath).Delete();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro está sendo gerado aqui: " + e);
            return false;
        }
    }

    private string GenerateNewFileName(string fileName, EPathAccess pathAccess, EUploadPath uploadPath, int limit = 255)
    {
        var guid = Guid.NewGuid().ToString("N");
        var clearedFileName = fileName.Replace("-", "");
        var url = GetFileUrl($"{guid}_{clearedFileName}", pathAccess, uploadPath);

        if (url.Length <= limit)
        {
            return $"{guid}_{clearedFileName}";
        }

        var remove = url.Length - limit;
        clearedFileName =
            clearedFileName.Remove(clearedFileName.Length - remove - Path.GetDirectoryName(clearedFileName).Length,
                remove);

        return $"{guid}_{clearedFileName}";
    }

    private string MountFilePath(string fileName, EUploadPath uploadPath)
    {
        
        var path = _uploadSettings.PublicBasePath;
        if (!path.EndsWith(Path.DirectorySeparatorChar))
        {
            path += Path.DirectorySeparatorChar;
        }
        return Path.Combine(path, uploadPath.ToDescriptionString(), fileName);
    }

    private string GetFileUrl(string fileName, EPathAccess pathAccess, EUploadPath uploadPath)
    {
        var baseUrl = _appSettings.UrlApi;
        if (!baseUrl.EndsWith('/'))
        {
            baseUrl += "/";
        }
        var fileUrl = Path.Combine(baseUrl, pathAccess.ToDescriptionString(), uploadPath.ToDescriptionString(), fileName);
        
        return fileUrl.Replace("\\", "/");       
    }

    private string GetFilePath(Uri uri)
    {
        var filePath = uri.AbsolutePath;
        if (filePath.StartsWith('/'))
        {
            filePath = filePath.Remove(0, 1);
        }

        var pathAccessStr = filePath.Split("/")[1];
        var pathAccess = Enum.Parse<EPathAccess>(pathAccessStr, true);
        filePath = filePath.Remove(0, pathAccess.ToDescriptionString().Length);
        if (filePath.StartsWith('/'))
        {
            filePath = filePath.Remove(0, 1);
        }

        var basePath = _uploadSettings.PublicBasePath;
        return Path.Combine(basePath, filePath);
    }

    private static byte[] ConvertFileInByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}