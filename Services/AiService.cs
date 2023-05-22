using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace ImageGeneratorApi.Services;

public class AiService
{
    private readonly OpenAIService _openAiService;

    public AiService(IConfiguration config)
    {
        _openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = config.GetConnectionString("OpenAiApiKey") ?? throw new Exception("OpenAiApiKey not found")
        });
    }

    public string CreateImage(string prompt)
    {
        var imageResult = _openAiService.Image.CreateImage(new ImageCreateRequest
        {
            Prompt = prompt,
            N = 2,
            Size = StaticValues.ImageStatics.Size.Size512,
            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            User = "ImageGeneratorUser"
        });

        return imageResult.Result.Results.First().Url;
    }
}