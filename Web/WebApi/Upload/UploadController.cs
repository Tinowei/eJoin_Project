using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using  Microsoft.AspNetCore.Http;
using Web.WebApiServices.UploadServices;

namespace Web.WebApi.Upload;
[Route("api/[controller]/[Action]")]
[ApiController]

public class UploadController : ControllerBase
{
    private readonly UploadImageService _uploadImageService;
    
    public UploadController(UploadImageService uploadImageService)
    {
        _uploadImageService = uploadImageService;
    }
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            string imageUrl = await _uploadImageService.UploadImageAsync(file);
            return Ok(new { url = imageUrl });
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"上傳時出現錯誤,錯誤訊息：{ex.Message}");
        }
        catch (Exception error)
        {
            return StatusCode(500, $"上傳時出現錯誤,錯誤訊息：{error.Message}");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
    {
        try
        {
            List<string> imageUrls = await _uploadImageService.UploadImagesAsync(files);
            return Ok(new { urls = imageUrls });
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"上傳時出現錯誤,錯誤訊息：{ex.Message}");
        }
        catch (Exception error)
        {
            return StatusCode(500, $"上傳時出現錯誤,錯誤訊息：{error.Message}");
        }
    }
}