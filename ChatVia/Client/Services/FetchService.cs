using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;

namespace ChatVia.Client.Services
{
    public class FetchService : IFetchService
    {
        private readonly IHttpClientFactory _httpClient;

        public FetchService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetAsync<T>(string url,
            Dictionary<string, string>? headers = null,
            string? customClient = null,
            bool includeCredentials = true, 
            Action<T>? callback = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            // Adding the headers
            if(headers is not null)
            {
                foreach(var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            if(includeCredentials)
            { 
                httpRequestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            }

            var client = _httpClient.CreateClient(customClient ?? "chatvia-api");
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions() 
                { PropertyNameCaseInsensitive = true, MaxDepth = int.MaxValue, };

            try
            {
                var response = 
                    await JsonSerializer.DeserializeAsync<T>(contentStream, options);

                if (response is not null)
                {
                    callback?.Invoke(response);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("IFetchSerivce: " + exp.Message);
            }
        }

        public async Task PostAsync<T>(string url, 
            Dictionary<string, string>? headers = null,
            object? body = null, 
            string? customClient = null, 
            bool includeCredentials = true, 
            Action<T>? callback = null)
        {
            var httpContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(body),
                System.Text.Encoding.UTF8,
                "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = httpContent,
            };

            if(headers is not null)
            {
                // Adding the headers
                foreach(var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            if(includeCredentials)
            { 
                httpRequestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            }

            var client = _httpClient.CreateClient(customClient ?? "chatvia-api");

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            // var content = await httpResponseMessage.Content.ReadAsStringAsync();
            // Console.WriteLine(content);
            // var response = JsonConvert.DeserializeObject<T>(content);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions() 
                { PropertyNameCaseInsensitive = true, MaxDepth = int.MaxValue,  };

            try
            {
                var response = 
                    await JsonSerializer.DeserializeAsync<T>(contentStream, options);

                if (response is not null)
                {
                    callback?.Invoke(response);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("IFetchSerivce: " + exp.Message);
            }
        }
    }
}
