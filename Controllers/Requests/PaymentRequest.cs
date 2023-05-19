using Microsoft.Build.Framework;

namespace ImageGeneratorApi.Controllers.Requests;

public class PaymentRequest
{
    public PaymentRequest(string email)
    {
        Email = email;
    }

    [Required] public string Email { get; set; }
}