using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace ImageGeneratorApi.Services;

public class StorageService
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName = "image-generator-with-dall-e";

    public StorageService(IConfiguration config)
    {
        var accessKey = config.GetConnectionString("AWSAccessKey") ?? throw new Exception("AWSAccessKey not found");
        var secretKey = config.GetConnectionString("AWSSecretKey") ?? throw new Exception("AWSSecretKey not found");

        _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.EUWest3);
    }

    public async Task UploadFileAsync(string filePath)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(filePath);
        response.EnsureSuccessStatusCode();
        var bytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = $"{Guid.NewGuid()}.jpeg";

        // Request to upload file and make public
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            ContentType = "image/jpg",
            InputStream = new MemoryStream(bytes),
            CannedACL = S3CannedACL.PublicRead
        };

        await _client.PutObjectAsync(request);
    }


    public async Task<List<string>> GetImagesUrlsAsync()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName
        };

        var response = await _client.ListObjectsV2Async(request);
        response.S3Objects.ForEach(x => x.Key = $"https://{_bucketName}.s3.eu-west-3.amazonaws.com/{x.Key}");
        return response.S3Objects.Select(x => x.Key).ToList();
    }
}