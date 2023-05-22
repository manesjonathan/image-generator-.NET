using ImageGeneratorApi.Controllers.Requests;
using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ImageGeneratorApi.Controllers;

[ApiController]
public class CheckoutController : Controller
{
    private readonly IConfiguration _config;
    private readonly AuthService _authService;
    private readonly KeyValuePair<int, int> _optionOne = new(100, 5);

    public CheckoutController(AuthService authService)
    {
        _authService = authService;
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

        // check if existing Stripe customer
        var stripeList = await customerService.ListAsync(new CustomerListOptions
        {
            Email = request.Email
        });

        var customer = stripeList.Data.FirstOrDefault() ?? await customerService.CreateAsync(customerOptions);
        var ephemeralKeyOptions = new EphemeralKeyCreateOptions
        {
            Customer = customer.Id
        };
        var ephemeralKeyService = new EphemeralKeyService();
        var ephemeralKey = await ephemeralKeyService.CreateAsync(ephemeralKeyOptions,
            new RequestOptions { ApiKey = StripeConfiguration.ApiKey });

        var paymentIntentOptions = new PaymentIntentCreateOptions
        {
            Amount = _optionOne.Key,
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
                Request.Headers["Stripe-Signature"], _config.GetConnectionString("WebhookSecret"));

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
        _authService.UpdateUserBucket(paymentIntent.ReceiptEmail, _optionOne.Value);
        return Ok();
    }
}