using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

public class NewPaymentController : RestfulControllerBase<NewPaymentController>
{
    private readonly IFasPaymentProcessor _paymentProcessor;

    public NewPaymentController(ILogger<NewPaymentController> logger, IFasPaymentProcessor paymentProcessor) : base(logger)
    {
        _paymentProcessor = paymentProcessor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newPaymentIm"></param>
    /// <returns></returns>
    [Produces(typeof(InitialisedPaymentLinkVm))]
    [HttpPost]
    public IActionResult Post([FromBody] NewPaymentIm newPaymentIm)
    {

        var sessionUrl = _paymentProcessor.CreatePaymentSession(new FasPayment()
        {
            Mode = FasPaymentMode.Subscription,
            Products = new List<FasProduct>()
            {
                new FasProduct()
                {
                    BillingPeriod = FasBillingPeriod.Monthly,
                    Currency = newPaymentIm.Currency,
                    Quantity = newPaymentIm.Quantity,
                    PaymentTitleOrProductName = newPaymentIm.ProductName,
                    Price = newPaymentIm.Price,
                    Interval = newPaymentIm.Interval,
                    IntervalCount = newPaymentIm.IntervalCount
                }
            }
        }).SessionUrl;

        return Ok(new InitialisedPaymentLinkVm(sessionUrl));
    }
}