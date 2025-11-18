using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Models.Payments;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers;

[Authorize]
public class InvoiceDataController : RestfulControllerBase<InvoiceDataController>
{
    private readonly IInvoiceDataService _invoiceDataService;

    public InvoiceDataController(ILogger<InvoiceDataController> logger, IInvoiceDataService invoiceDataService) : base(logger)
    {
        _invoiceDataService = invoiceDataService;
    }

    [HttpPost]
    [Produces(typeof(InvoiceDataVm))]
    public IActionResult Post([FromBody] InvoiceDataIm invoiceDataIm)
    {
        InvoiceDataVm result = _invoiceDataService.Create(invoiceDataIm);
        return Ok(result);
    }

    [HttpGet]
    [Produces(typeof(InvoiceDataVm))]
    public IActionResult Get(Guid companyGuid)
    {
        InvoiceDataVm result = _invoiceDataService.Get(companyGuid);
        return Ok(result);
    }

    [HttpPut]
    [Produces(typeof(InvoiceDataVm))]
    public IActionResult Put([FromBody] InvoiceDataUm invoiceDataUm)
    {
        InvoiceDataVm result = _invoiceDataService.Update(invoiceDataUm);
        return Ok(result);
    }
}