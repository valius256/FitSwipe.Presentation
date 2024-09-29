
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public static async Task<ReturnType?> PostAsync<RequestType, ReturnType>(string url, RequestType body, string token = "")
        {
            try
            {
                // Send POST request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonSerializer.Serialize(body);
                // Create the content object for the POST request (with UTF8 encoding and application/json content type)
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(Constant.BaseUrl + url, content);

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read response as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content into the specified type
                ReturnType? result = JsonSerializer.Deserialize<ReturnType>(responseContent, new JsonSerializerOptions
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
        public static async Task PostAsync<RequestType>(string url, RequestType body, string token = "")
        {
            try
            {
                // Send POST request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonSerializer.Serialize(body);
                // Create the content object for the POST request (with UTF8 encoding and application/json content type)
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(Constant.BaseUrl + url, content);

                // Ensure success status code
                response.EnsureSuccessStatusCode();
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
        public static async Task PutAsync<RequestType>(string url, RequestType body, string token = "")
        {
            try
            {
                // Send Put request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonSerializer.Serialize(body);
                // Create the content object for the PUT request (with UTF8 encoding and application/json content type)
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(Constant.BaseUrl + url, content);
                response.EnsureSuccessStatusCode();
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
        public static async Task PatchAsync<RequestType>(string url, RequestType body, string token = "")
        {
            try
            {
                // Send PATCH request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonSerializer.Serialize(body);
                // Create the content object for the PATCH request (with UTF8 encoding and application/json content type)
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PatchAsync(Constant.BaseUrl + url, content);
                response.EnsureSuccessStatusCode();
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
        public static async Task DeleteAsync<RequestType>(string url, string token = "")
        {
            try
            {
                // Send DELETE request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response =  await _httpClient.DeleteAsync(Constant.BaseUrl + url);
                response.EnsureSuccessStatusCode();
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
