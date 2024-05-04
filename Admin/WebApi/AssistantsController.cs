using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Models.Settings;
using Admin.DTOs;
using System.Text.Json;

namespace Admin.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssistantsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenAISettings _openAiSettings;
        
        
        public AssistantsController(IHttpClientFactory httpClientFactory, OpenAISettings openAiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _openAiSettings = openAiSettings;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssistant([FromBody] CreateAssistantRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);

            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
            
            //openAI要的檔案格式
            var payload = new
            {
                name = request.Name,
                instructions = request.Instructions,
                model = "gpt-4-turbo",
                tools = new[] { new { type = "retrieval" } },
                file_ids = request.FileIds
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_openAiSettings.AssistantsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(new
                {
                    IsSuccess = true,
                    Body = responseString,
                    Message = "Assistant created successfully."
                });
            }

            {
                return Ok(new
                {
                    IsSuccess = false,
                    Message = "Failed to create an assistant"
                });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAssistants()
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);

            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");

            var response = await client.GetAsync(_openAiSettings.AssistantsUrl);

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
            else
            {
                return Ok(new
                {
                    IsSuccess = false,
                    Message = "Failed to upload file."
                });
            }
        }
        
        
        
        //向OpenAi打一個刪除檔案的Api
        [HttpPost]
        public async Task<IActionResult> DeleteAssistant([FromBody] DeleteAssistantRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);

            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");

            var endpoint = $"{_openAiSettings.AssistantsUrl}/{request.AssistantId}";
            var response = await client.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(new
                {
                    IsSuccess = true,
                    Body = responseString,
                    Message = "Assistant deleted successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    IsSuccess = false,
                    Message = "Failed to delete an assistant"
                });
            }
        }
        
        
        
    }
}
