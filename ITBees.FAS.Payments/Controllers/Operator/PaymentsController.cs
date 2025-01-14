using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentsController : RestfulControllerBase<PaymentsController>
{
    private readonly IPaymentServiceInfo _paymentServiceInfo;

    public PaymentsController(ILogger<PaymentsController> logger, IPaymentServiceInfo paymentServiceInfo) : base(logger)
    {
        _paymentServiceInfo = paymentServiceInfo;
    }

    [HttpGet]
    [Produces<PaginatedResult<PaymentVm>>]
    public IActionResult Get(bool viewAsHtml, string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        if (viewAsHtml)
        {
            var result = _paymentServiceInfo.Get(authKey, page, pageSize, sortColumn, sortOrder);

            var html = new StringBuilder();

            html.Append("<html><body>");

            // Form for authKey and pagination parameters
            html.Append("<form method='get'>");
            html.Append("Auth Key: <input type='text' name='authKey' value='" + (authKey ?? "") + "' /> ");
            html.Append("<input type='hidden' name='page' value='" + result.CurrentPage + "' />");
            html.Append("<input type='hidden' name='pageSize' value='" + result.ElementsPerPage + "' />");
            html.Append("<input type='submit' value='Submit' />");
            html.Append("</form><br/>");

            // Table of payments
            html.Append("<table border='1'>");
            html.Append("<thead><tr>");
            html.Append("<th>Payment Session Guid</th>");
            html.Append("<th>Created</th>");
            html.Append("<th>Finished</th>");
            html.Append("<th>Success</th>");
            html.Append("<th>Finished Date</th>");
            html.Append("<th>Operator Transaction Id</th>");
            html.Append("<th>Payment Operator</th>");
            html.Append("<th>Email</th>");
            html.Append("<th>Value</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            foreach (var item in result.Data)
            {
                html.Append("<tr>");
                html.Append("<td>" + item.PaymentSessionGuid + "</td>");
                html.Append("<td>" + item.Created + "</td>");
                html.Append("<td>" + item.Finished + "</td>");
                html.Append("<td>" + item.Success + "</td>");
                html.Append("<td>" + (item.FinishedDate?.ToString() ?? "") + "</td>");
                html.Append("<td>" + (item.OperatorTransactionId ?? "") + "</td>");
                html.Append("<td>" + (item.PaymentOperator ?? "") + "</td>");
                html.Append("<td>" + item.Email + "</td>");
                html.Append("<td>" + item.Value.ToString("F2") + "</td>");
                html.Append("</tr>");
            }

            html.Append("</tbody></table>");

            // Pagination controls
            html.Append("<div>");
            html.Append($"Page {result.CurrentPage} of {result.AllPagesCount}<br/>");

            if (result.CurrentPage > 1)
            {
                html.Append($"<a href='?authKey={authKey}&page={result.CurrentPage - 1}&pageSize={result.ElementsPerPage}'>Previous</a> ");
            }

            if (result.CurrentPage < result.AllPagesCount)
            {
                html.Append($"<a href='?authKey={authKey}&page={result.CurrentPage + 1}&pageSize={result.ElementsPerPage}'>Next</a>");
            }

            html.Append("</div>");

            html.Append("</body></html>");

            return Content(html.ToString(), "text/html");
        }
        else
        {
            return ReturnOkResult(() => _paymentServiceInfo.Get(authKey, page, pageSize, sortColumn, sortOrder));
        }
    }
}