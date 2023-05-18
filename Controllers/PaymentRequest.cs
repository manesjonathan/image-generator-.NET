using Microsoft.Build.Framework;

namespace ImageGeneratorApi.Controllers;

public class PaymentRequest
{
    public PaymentRequest(string email)
    {
        Email = email;
    }

    [Required] public string Email { get; set; }
}