using System.Net.Http.Headers;
using System.Text;
using Admin.DTOs;
using Admin.Models.Settings;
using Newtonsoft.Json.Linq;


namespace Admin.Services;

public class EventConsultantService
{
    private readonly string _assistantId;
    private readonly OpenAISettings _openAiSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public EventConsultantService(LineBotSettings lineBotSettings, OpenAISettings openAiSettings,
        IHttpClientFactory httpClientFactory)
    {
        _openAiSettings = openAiSettings;
        _httpClientFactory = httpClientFactory;
        _assistantId = lineBotSettings.OpenAIAssistantId;
    }
    
    /// <summary>
    /// 拿到訊息 -> 創建一個Thread -> add Message -> run Thread
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task<string> GetSingleResponseFromAssistant(string message)
    {
        var currentRun = await CreateThreadAndRun(message); 
        await CheckRunStatus(currentRun.ThreadId, currentRun.RunId); //問回覆狀態
        var resultMessage = await GetMessageByThreadId(currentRun.ThreadId);  //拿第一個回覆
        await RemoveThreadByThreadId(currentRun.ThreadId); //刪掉這次的Thread , 因為此設計是沒有記憶對話紀錄的
        return resultMessage;
    }
    
    private async Task<OpenAiAssistantRun> CreateThreadAndRun(string message)
    {
        
        var client = CreateHttpClient();
        var endpoint = _openAiSettings.AssistantRunAPIUrl;

        var payload = new
        {
            assistant_id = _assistantId,
            thread = new
            {
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = message
                    }
                }
            },
            temperature = 0.1
        };
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
        Console.WriteLine(endpoint);
        Console.WriteLine(jsonPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);

        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(responseContent);

        return new OpenAiAssistantRun
        {
            RunId = obj["id"].ToString(),
            ThreadId = obj["thread_id"].ToString()
        };
        
        
    }
    /// <summary>
    /// 重複的去訪問OpenAi 訊息是否處理好了
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <exception cref="Exception"></exception>
    private async Task CheckRunStatus(string threadId, string runId)
    {
        var client = CreateHttpClient();
        var endpoint = $"{_openAiSettings.AssistantThreadAPIUrl}/{threadId}/runs/{runId}";
        int retryCount = 0;
        const int maxRetries = 10;
        const int retryDelay = 5;

        while (true)
        {
            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode && retryCount < maxRetries)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(responseContent);
                var status = obj["status"].ToString();

                if (status == "completed")
                {
                    return;
                }
            }
            else if (retryCount >= maxRetries)
            {
                throw new Exception($"Run failed after {maxRetries} attempts");
            }

            retryCount++;
            await Task.Delay(TimeSpan.FromSeconds(retryDelay));
        }
    }
    
    private async Task<string> GetMessageByThreadId(string threadId)
    {
        var client = CreateHttpClient();
        var endpoint = $"{_openAiSettings.AssistantThreadAPIUrl}/{threadId}/messages";
        var response = await client.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JObject.Parse(responseContent);
        return result["data"][0]["content"][0]["text"]["value"].ToString();
    }
    
    private async Task RemoveThreadByThreadId(string threadId)
    {
        var client = CreateHttpClient();
        var endpoint = $"{_openAiSettings.AssistantThreadAPIUrl}/{threadId}";
        var response = await client.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }

    private HttpClient CreateHttpClient()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiSettings.ApiKey);
        client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        return client;
    }
}