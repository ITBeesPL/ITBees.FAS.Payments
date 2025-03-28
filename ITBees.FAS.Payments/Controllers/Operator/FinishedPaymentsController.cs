﻿using ITBees.FAS.Payments.Interfaces;
using ITBees.Interfaces.Repository;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class FinishedPaymentsController : RestfulControllerBase<FinishedPaymentsController>
{
    private readonly IPaymentServiceInfo _paymentServiceInfo;

    public FinishedPaymentsController(ILogger<FinishedPaymentsController> logger,
        IPaymentServiceInfo paymentServiceInfo) : base(logger)
    {
        _paymentServiceInfo = paymentServiceInfo;
    }

    [HttpGet]
    [Produces<PaginatedResult<FinishedPaymentVm>>]
    public IActionResult Get(bool viewAsHtml, string? authKey, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder, int? month, int? year)
    {
        var result = _paymentServiceInfo.GetFinishedPayments(authKey, page, pageSize, sortColumn, sortOrder);
        if (month != null && year != null)
        {
            result.Data = result.Data.Where(x => x.Created.Value.Month == month && x.Created.Value.Year == year).ToList();
        }
        
        decimal totalAmount = result.Data.Sum(x=>Convert.ToDecimal(x.Amount));

        if (viewAsHtml)
        {
            var html = new StringBuilder();

            html.Append("<html><head><meta charset=\"UTF-8\"></head><body>");

            // Form for authKey, month, year, and pagination parameters
            html.Append("<form method='get'>");
            html.Append("Auth Key: <input type='text' name='authKey' value='" + (authKey ?? "") + "' /> ");
            html.Append("Month: <select name='month'>");
            for (int i = 1; i <= 12; i++)
            {
                html.Append($"<option value='{i}' " + (month == i ? "selected" : "") + $">{i}</option>");
            }
            html.Append("</select> ");
            html.Append("Year: <input type='number' name='year' value='" + (year ?? DateTime.Now.Year) + "' /> ");
            html.Append("<input type='hidden' name='page' value='" + result.CurrentPage + "' />");
            html.Append("<input type='hidden' name='pageSize' value='" + result.ElementsPerPage + "' />");
            html.Append("<input type='hidden' name='viewAsHtml' value='true' />");
            html.Append("<input type='submit' value='Submit' />");
            html.Append("</form><br/>");

            // Table of finished payments
            html.Append("<table border='1'>");
            html.Append("<thead><tr>");
            html.Append("<th>Created</th>");
            html.Append("<th>Finished</th>");
            html.Append("<th>City</th>");
            html.Append("<th>NIP</th>");
            html.Append("<th>Street</th>");
            html.Append("<th>Country</th>");
            html.Append("<th>Company Name</th>");
            html.Append("<th>Email</th>");
            html.Append("<th>Post Code</th>");
            html.Append("<th>Invoice Requested</th>");
            html.Append("<th>Amount</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            foreach (var item in result.Data)
            {
                html.Append("<tr>");
                html.Append("<td>" + item.Created + "</td>");
                html.Append("<td>" + (item.Finished.Value ? "Yes" : "No") + "</td>");
                html.Append("<td>" + item.City + "</td>");
                html.Append("<td>" + item.Nip + "</td>");
                html.Append("<td>" + item.Street + "</td>");
                html.Append("<td>" + item.Country + "</td>");
                html.Append("<td>" + item.CompanyName + "</td>");
                html.Append("<td>" + item.Email + "</td>");
                html.Append("<td>" + item.PostCode + "</td>");
                var yes = item.InvoiceRequested == null ? "" : (item.InvoiceRequested.Value ? "Yes" : "No");
                html.Append("<td>" + yes + "</td>");
                html.Append("<td>" + item.Amount + "</td>");
                html.Append("</tr>");
            }

            AppendSummaryRow(html, totalAmount.ToString("F2"));

            html.Append("</tbody></table>");


            // Pagination controls
            html.Append("<div>");
            html.Append($"Page {result.CurrentPage} of {result.AllPagesCount}<br/>");

            if (result.CurrentPage > 1)
            {
                html.Append($"<a href='?authKey={authKey}&month={month}&year={year}&page={result.CurrentPage - 1}&pageSize={result.ElementsPerPage}'>Previous</a> ");
            }

            if (result.CurrentPage < result.AllPagesCount)
            {
                html.Append($"<a href='?authKey={authKey}&month={month}&year={year}&page={result.CurrentPage + 1}&pageSize={result.ElementsPerPage}'>Next</a>");
            }

            html.Append("</div>");

            html.Append("</body></html>");

            return Content(html.ToString(), "text/html");
        }
        else
        {
            return ReturnOkResult(() => _paymentServiceInfo.GetFinishedPayments(authKey, page, pageSize, sortColumn, sortOrder));
        }
    }

    private void AppendSummaryRow(StringBuilder html, string value)
    {
        html.Append("<tr>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td>" + "</td>");
        html.Append("<td></td>");
        html.Append("<td></td>");
        html.Append("<td>Summary</td>");
        html.Append("<td>" + value + "</td>");
        html.Append("</tr>");
    }
}