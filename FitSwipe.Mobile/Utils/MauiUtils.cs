
using FitSwipe.Shared.Dtos.Upload;
using FitSwipe.Shared.Utils;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FitSwipe.Mobile.Utils
{
    public static class MauiUtils
    {
        public async static Task<GetUploadResultDto?> UploadImageAsync(FileResult photo, string token)
        {
            // Convert the photo to a Stream
            var stream = await photo.OpenReadAsync();

            // Prepare HttpClient
            using var client = new HttpClient();

            // Prepare the content for the multipart request
            using var content = new MultipartFormDataContent();

            // Convert stream to ByteArrayContent
            var imageContent = new StreamContent(stream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // or other appropriate type

            // Add file to content (the "file" name must match the API parameter)
            content.Add(imageContent, "file", photo.FileName);

            // Set up the API URL
            var url = Constant.BaseUrl + "api/UploadDowload/upload-image?fileName=" + photo.FileName;

            //Authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send the POST request
            var response = await client.PostAsync(url, content);
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<GetUploadResultDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Optional: ignore case for property names
            });
            // Ensure the response is successful
            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new Exception($"Failed to upload image. Status code: {response.StatusCode}");
            }
        }
    }
}
