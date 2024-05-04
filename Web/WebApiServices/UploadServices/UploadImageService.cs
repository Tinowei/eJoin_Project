using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using  Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;



namespace Web.WebApiServices.UploadServices;

public class UploadImageService
{
    private readonly Cloudinary _cloudinary;
    private static readonly HashSet<string> _supportedFileTypes = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
    public UploadImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        ValidateFile(file);
        var uploadParams = CreateUploadParams(file);
        var uploadResult = await UploadFileAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }

    public async Task<List<string>> UploadImagesAsync(List<IFormFile> files)
    {
        if (files.Count == 0 || files == null)
        {
            throw new ArgumentException("未收到檔案。");
        }
        var imageUrls = new List<string>();
        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                continue;
            }
            ValidateFile(file);
            var uploadParams = CreateUploadParams(file);
            var uploadResult = await UploadFileAsync(uploadParams);
            imageUrls.Add(uploadResult.SecureUrl.ToString());
        }
        return imageUrls;
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            throw new ArgumentException("傳進的publicId是空的");
        }

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image
        };
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        if (deletionResult.Result == "ok")
        {
            return true;
        }

        throw new Exception($"刪除發生錯誤{deletionResult.Error?.Message}");

    }


    private void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new AggregateException("未收到檔案");
        }
        
        var fileType = Path.GetExtension(file.FileName).ToLower();
        if (!_supportedFileTypes.Contains(fileType))
        {
            throw new ArgumentException("Unsupported file type.");
        }
    }

    private ImageUploadParams CreateUploadParams(IFormFile file)
    {
        return new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = "ImageUploadByApi"
        };
    }
    
    private async Task<UploadResult> UploadFileAsync(ImageUploadParams uploadParams)
    {
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new Exception($"上傳時出現錯誤,錯誤訊息：{uploadResult.Error.Message}");
        }
        return uploadResult;
    }
}

//先切小部分。
//判斷檔案類型
//向cloudinary發api -> 這裡只有cloudinary實作的才應該抽成介面
//拿到cloudinary的result後應該要做什麼？