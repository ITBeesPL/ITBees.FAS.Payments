using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentsLogsController : RestfulControllerBase<PaymentsLogsController>
{
    private readonly IPaymentServiceInfo _paymentServiceInfo;

    public PaymentsLogsController(ILogger<PaymentsLogsController> logger, IPaymentServiceInfo paymentServiceInfo) : base(logger)
    {
        _paymentServiceInfo = paymentServiceInfo;
    }

    [HttpGet]
    [Produces<PaginatedResult<PaymentLogVm>>]
    public IActionResult Get(bool viewAsHtml, string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder)
    {
        if (viewAsHtml)
        {
            var result = _paymentServiceInfo.GetLogs(authKey, page, pageSize, sortColumn, sortOrder);

            var html = new StringBuilder();

            html.Append("<html><body>");

            // Form for authKey and pagination parameters
            html.Append("<form method='get'>");
            html.Append("Auth Key: <input type='text' name='authKey' value='" + (authKey ?? "") + "' /> ");
            html.Append("<input type='hidden' name='page' value='" + result.CurrentPage + "' />");
            html.Append("<input type='hidden' name='pageSize' value='" + result.ElementsPerPage + "' />");
            html.Append("<input type='submit' value='Submit' />");
            html.Append("</form><br/>");

            // Table of payment logs
            html.Append("<table border='1'>");
            html.Append("<thead><tr>");
            html.Append("<th>Id</th>");
            html.Append("<th>Received</th>");
            html.Append("<th>Event</th>");
            html.Append("<th>Operator</th>");
            html.Append("<th>Json Event</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            foreach (var item in result.Data)
            {
                html.Append("<tr>");
                html.Append("<td>" + item.Id + "</td>");
                html.Append("<td>" + item.Received + "</td>");
                html.Append("<td>" + item.Event + "</td>");
                html.Append("<td>" + item.Operator + "</td>");
                html.Append("<td>" + item.JsonEvent + "</td>");
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
            return ReturnOkResult(() => _paymentServiceInfo.GetLogs(authKey, page, pageSize, sortColumn, sortOrder));
        }
    }
}