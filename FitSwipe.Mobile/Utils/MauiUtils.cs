
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
        public async static Task<GetUploadResultDto?> UploadVideoAsync(FileResult video, string token)
        {
            // Convert the video to a Stream
            var stream = await video.OpenReadAsync();

            // Prepare HttpClient
            using var client = new HttpClient();

            // Prepare the content for the multipart request
            using var content = new MultipartFormDataContent();

            // Convert stream to ByteArrayContent
            var videoContent = new StreamContent(stream);
            videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4"); // or other appropriate type (e.g., video/avi, video/mkv)

            // Add file to content (the "file" name must match the API parameter)
            content.Add(videoContent, "file", video.FileName);

            // Set up the API URL
            var url = Constant.BaseUrl + "api/UploadDowload/upload-video?fileName=" + video.FileName;

            // Authentication
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
                throw new Exception($"Failed to upload video. Status code: {response.StatusCode}");
            }
        }
        //private async static Task<GetUploadResultDto?> UploadImageFromStreamAsync(Stream imageStream, string fileName, string token)
        //{
        //    // Prepare HttpClient
        //    using var client = new HttpClient();

        //    // Prepare the content for the multipart request
        //    using var content = new MultipartFormDataContent();

        //    // Convert stream to ByteArrayContent
        //    var imageContent = new StreamContent(imageStream);
        //    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // or other appropriate type

        //    // Add file to content (the "file" name must match the API parameter)
        //    content.Add(imageContent, "file", fileName);

        //    // Set up the API URL
        //    var url = Constant.BaseUrl + "api/UploadDowload/upload-image?fileName=" + fileName;

        //    // Authentication
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    // Send the POST request
        //    var response = await client.PostAsync(url, content);
        //    var json = await response.Content.ReadAsStringAsync();

        //    var result = JsonSerializer.Deserialize<GetUploadResultDto>(json, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true // Optional: ignore case for property names
        //    });

        //    // Ensure the response is successful
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        throw new Exception($"Failed to upload image. Status code: {response.StatusCode}");
        //    }
        //}
        ///// <summary>
        ///// Still under testing
        ///// </summary>
        ///// <param name="video"></param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentException"></exception>
        //public async static Task<GetUploadVideoResultDto?> UploadVideoAndExtractThumbnailAsync(FileResult video, string token)
        //{
        //    if (video == null)
        //        throw new ArgumentException("Video file is null");

        //    // Convert the video to a Stream
        //    var stream = await video.OpenReadAsync();

        //    // Temporary file path to save the video stream (can be used for FFmpeg processing)
        //    var tempVideoFilePath = Path.Combine(Path.GetTempPath(), video.FileName);

        //    // Save the stream to a temporary file
        //    using (var fileStream = new FileStream(tempVideoFilePath, FileMode.Create))
        //    {
        //        await stream.CopyToAsync(fileStream);
        //    }

        //    // Extract a thumbnail from the video using FFmpeg or any other library
        //    var thumbnailStream = ExtractThumbnailFromVideo(tempVideoFilePath);

        //    // Upload the video
        //    var videoUploadResult = await UploadVideoAsync(video, token);

        //    // Upload the extracted thumbnail image
        //    var thumbnailUploadResult = await UploadImageFromStreamAsync(thumbnailStream, "thumbnail.jpeg", token);

        //    // Clean up temporary files
        //    File.Delete(tempVideoFilePath);

        //    // Return both the video and thumbnail URLs (or other relevant results)
        //    return new GetUploadVideoResultDto
        //    {
        //        VideoUrl = videoUploadResult?.FileUrl,
        //        ThumbnailUrl = thumbnailUploadResult?.FileUrl
        //    };
        //}

        //private static Stream ExtractThumbnailFromVideo(string videoFilePath)
        //{
        //    // Assuming FFmpeg is available on your system. You might need to adjust this based on the library you're using.
        //    using (var engine = new Engine())
        //    {
        //        var videoFile = new MediaFile { Filename = videoFilePath };
        //        engine.GetMetadata(videoFile);

        //        // Extract a thumbnail (e.g., at 0.5 second) to memory
        //        using (var thumbnailStream = new MemoryStream())
        //        {
        //            var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(0.5) };
        //            var thumbnailFile = new MediaFile { Filename = null }; // No filename is needed

        //            engine.GetThumbnail(videoFile, thumbnailFile, options);

        //            // Reset the stream position for further use
        //            thumbnailStream.Position = 0;

        //            return thumbnailStream;
        //        }
        //    }
        //}


    }
}
