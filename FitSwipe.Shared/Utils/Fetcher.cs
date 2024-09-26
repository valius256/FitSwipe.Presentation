
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FitSwipe.Shared.Utils
{
    public static class Fetcher
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<T?> GetAsync<T>(string url, string token = "")
        {
            try
            {
                // Send GET request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.GetAsync(Constant.BaseUrl + url);

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read response as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content into the specified type
                T? result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Optional: ignore case for property names
                });

                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Serialization error: {e.Message}");
                throw;
            }
        }
    }
}
