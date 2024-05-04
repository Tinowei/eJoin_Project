using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Admin.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenAISettings _openAiSettings;
        
        public FileUploadController(IHttpClientFactory httpClientFactory, OpenAISettings openAiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _openAiSettings = openAiSettings;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileToOpenAi(IFormFile file)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);
            
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent("assistants"), "purpose");
                await using (var fileStream = file.OpenReadStream())
                {
                    content.Add(new StreamContent(fileStream), "file", file.FileName);
                    var response = await client.PostAsync(_openAiSettings.FileUploadUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return Ok(new
                        {
                            IsSuccess = true,
                            Body = responseContent,
                            Message = "File uploaded successfully."
                        });
                    }

                    return Ok(new
                    {
                        IsSuccess = false,
                        Message = "Failed to upload file."
                    });
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilesFromOpenAI()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);
            
            var response = await client.GetAsync(_openAiSettings.GetAllFilesUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(new
                {
                    IsSuccess = true,
                    Body = responseContent,
                    Message = "Files retrieved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    IsSuccess = false,
                    Message = "Failed to retrieve files."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileFromOpenAI([FromQuery] string file_Id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);
            
            var url = $"{_openAiSettings.FileDeleteUrl}/{file_Id}";
        
            var response = await client.DeleteAsync(url);
            
            
            if (response.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    IsSuccess = true,
                    Message = "File deleted successfully."
                });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // 返回更具體的錯誤信息
                return BadRequest(new
                {
                    IsSuccess = false,
                    Message = $"Failed to delete file. {errorContent}"
                });
            }

        }
    }
}
