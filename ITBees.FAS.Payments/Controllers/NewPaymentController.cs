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

    public IActionResult Post([FromBody] NewPaymentIm newPaymentIm)
    {
        _paymentProcessor.CreatePaymentSession(new FasPayment()
        {
            Mode = FasPaymentMode.Subscription,
            Products = new List<FasProduct>()
        });

        return Ok();
    }
}

public class NewPaymentIm
{
    public Guid CompanyGuid{ get; set; }
    public decimal Price { get; set; }
    public Guid Type { get; set; }
    public long Quantity { get; set; }
    public string ProductName { get; set; }
    public Guid CustomerGuid { get; set; }
}