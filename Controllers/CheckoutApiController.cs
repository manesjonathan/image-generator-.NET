using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ImageGeneratorApi.Controllers;

[ApiController]
public class CheckoutApiController : Controller
{
    private readonly IConfiguration _config;
    private readonly UserService _userService;

    public CheckoutApiController(UserService userService)
    {
        _userService = userService;
        _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        StripeConfiguration.ApiKey = _config.GetConnectionString("StripeApiKey");
    }

    [HttpPost]
    [Route("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentSheet([FromBody] PaymentRequest request)
    {
        // Use an existing Customer ID if this is a returning customer.
        var customerOptions = new CustomerCreateOptions
        {
            Email = request.Email,
        };
        var customerService = new CustomerService();
        var customer = await customerService.CreateAsync(customerOptions);

        var ephemeralKeyOptions = new EphemeralKeyCreateOptions
        {
            Customer = customer.Id
        };
        var ephemeralKeyService = new EphemeralKeyService();
        var ephemeralKey = await ephemeralKeyService.CreateAsync(ephemeralKeyOptions,
            new RequestOptions { ApiKey = StripeConfiguration.ApiKey });

        var paymentIntentOptions = new PaymentIntentCreateOptions
        {
            Amount = 100,
            Currency = "eur",
            Customer = customer.Id,
            ReceiptEmail = customer.Email,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };
        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

        var response = new
        {
            paymentIntent = paymentIntent.ClientSecret,
            ephemeralKey = ephemeralKey.Secret,
            customer = customer.Id,
            publishableKey = _config.GetConnectionString("StripePublishableKey")
        };

        return Json(response);
    }

    [HttpPost]
    [Route("webhook")]
    public async Task<IActionResult> ProcessPayment()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], "whsec_fEjz4Ylg6HqORhn1IqOLiN8qlxwJqVCV");

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    // Then define and call a method to handle the successful payment intent.
                    HandlePaymentIntentSucceeded(paymentIntent);
                    break;
                case "payment_method.attached":
                    //var paymentMethod = (PaymentMethod)stripeEvent.Data.Object;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                    break;
                // ... handle other event types
                default:
                    // Unexpected event type
                    return BadRequest();
            }

            return Ok();
        }

        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private OkResult HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
    {
        Console.WriteLine(paymentIntent.ReceiptEmail);
        _userService.UpdateUserBucket(paymentIntent.ReceiptEmail, 2);
        return Ok();
    }
}