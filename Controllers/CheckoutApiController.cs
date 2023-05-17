using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ImageGeneratorApi.Controllers;

[ApiController]
public class CheckoutApiController : Controller
{
    public CheckoutApiController()
    {
        StripeConfiguration.ApiKey =
            "sk_test_51IjwEfC1js4lodAyTj8o34BJ7kUtbCOAMAJpaBijgfFVKpuzjHtKzcNONBbE3EPN3m8SNRPljK1XICjfqFhnJaDt00ruAaqgNK";
    }

    [HttpPost]
    [Route("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentSheet()
    {
        StripeConfiguration.ApiKey =
            "sk_test_51IjwEfC1js4lodAyTj8o34BJ7kUtbCOAMAJpaBijgfFVKpuzjHtKzcNONBbE3EPN3m8SNRPljK1XICjfqFhnJaDt00ruAaqgNK";

        // Use an existing Customer ID if this is a returning customer.
        var customerOptions = new CustomerCreateOptions();
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
            Amount = 1099,
            Currency = "eur",
            Customer = customer.Id,
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
            publishableKey =
                "pk_test_51IjwEfC1js4lodAyBRNVJ83bNJtsel67h00mea3VzENDLODQNxDERGAa9hQ9h4yptXh7ZXAnju179KJLgy2HekGd0096Mtmzjr"
        };

        return Json(response);
    }
}