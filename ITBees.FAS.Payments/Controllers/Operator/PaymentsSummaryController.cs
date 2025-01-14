using System.Text;
using ITBees.FAS.Payments.Services.Operator;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Controllers.Operator
{
    public class PaymentsSummaryController : RestfulControllerBase<PaymentsSummaryController>
    {
        private readonly IPaymentSummaryService _paymentSummaryService;

        public PaymentsSummaryController(
            ILogger<PaymentsSummaryController> logger,
            IPaymentSummaryService paymentSummaryService) : base(logger)
        {
            _paymentSummaryService = paymentSummaryService;
        }

        [HttpGet]
        [Produces(typeof(PaymentSummaryVm))] // For Swagger or API docs
        public IActionResult Get(
            bool viewAsHtml,
            string authKey,
            int? month,
            int? year,
            int commisionPercentage,
            int vatPercentage)
        {
            if (viewAsHtml)
            {
                var result = _paymentSummaryService.Get(authKey, month, year, commisionPercentage, vatPercentage);

                var html = new StringBuilder();
                html.Append("<html>");
                html.Append("<head>");
                // -- Include Chart.js from a CDN
                html.Append("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");

                // --- BEGIN JavaScript block ---
                html.Append(@"
                <script>
                    // This variable will hold the last known aggregated value for the newest month
                    let lastCheckValue = 0;

                    // We'll also hold a reference to the chart object for updating
                    let myChart = null;

                    // Once the page loads, we want to build the initial chart and set up periodic checks
                    window.onload = function() {
                        buildChart();
                        checkUpdates(); // check once at start
                        // Then check every 60 seconds (60000 ms)
                        setInterval(checkUpdates, 60000);
                    }

                    // ---------------------------
                    // buildChart()
                    // ---------------------------
                    // Fetches JSON data (viewAsHtml=false), aggregates amounts by (year, month),
                    // and renders the initial Chart.js chart.
                    function buildChart() {
                        let jsonUrl = window.location.href.replace('viewAsHtml=true','viewAsHtml=false');

                        fetch(jsonUrl)
                            .then(response => response.json())
                            .then(data => {
                                if (data && data.payments && data.payments.length > 0) {
                                    // Prepare grouped data
                                    // Key: 'YYYY/MM', Value: sum of totalGrossAmount
                                    let groupedMap = groupByYearMonth(data.payments);

                                    // Sort the keys chronologically (optional, if not guaranteed by server)
                                    let sortedKeys = Array.from(groupedMap.keys())
                                        .sort((a, b) => {
                                            // a = '2024/10', b = '2023/11', etc.
                                            let [yearA, monthA] = a.split('/').map(Number);
                                            let [yearB, monthB] = b.split('/').map(Number);
                                            // First compare year, then month
                                            if (yearA !== yearB) return yearA - yearB;
                                            return monthA - monthB;
                                        });

                                    // Build final labels and amounts arrays
                                    let labels = sortedKeys;
                                    let amounts = sortedKeys.map(k => groupedMap.get(k));

                                    // Store the last check value (the last aggregated month)
                                    if (amounts.length > 0) {
                                        lastCheckValue = amounts[amounts.length - 1];
                                    }

                                    let ctx = document.getElementById('myChart').getContext('2d');

                                    // If chart already exists, destroy it before re-creating
                                    if (myChart !== null) {
                                        myChart.destroy();
                                    }

                                    myChart = new Chart(ctx, {
                                        type: 'line',
                                        data: {
                                            labels: labels,
                                            datasets: [{
                                                label: 'Total Gross Amount (Aggregated)',
                                                data: amounts,
                                                borderColor: 'rgb(75, 192, 192)',
                                                fill: false
                                            }]
                                        },
                                        options: {
                                            responsive: true,
                                            title: {
                                                display: true,
                                                text: 'Payment Summary - Aggregated Gross Amount'
                                            },
                                            tooltips: {
                                                mode: 'index',
                                                intersect: false
                                            }
                                        }
                                    });
                                }
                            })
                            .catch(error => {
                                console.error('Error fetching JSON data for chart:', error);
                            });
                    }

                    // ---------------------------
                    // checkUpdates()
                    // ---------------------------
                    // Periodically fetches the JSON, re-aggregates data, checks if there's an increase
                    // in the last month, and updates the chart as needed.
                    function checkUpdates() {
                        let jsonUrl = window.location.href.replace('viewAsHtml=true','viewAsHtml=false');

                        fetch(jsonUrl)
                            .then(response => response.json())
                            .then(data => {
                                if (data && data.payments && data.payments.length > 0) {
                                    let groupedMap = groupByYearMonth(data.payments);

                                    // Sort keys to identify the last chronological month
                                    let sortedKeys = Array.from(groupedMap.keys())
                                        .sort((a, b) => {
                                            let [yearA, monthA] = a.split('/').map(Number);
                                            let [yearB, monthB] = b.split('/').map(Number);
                                            if (yearA !== yearB) return yearA - yearB;
                                            return monthA - monthB;
                                        });

                                    let amounts = sortedKeys.map(k => groupedMap.get(k));
                                    if (amounts.length === 0) return;

                                    // The newest month is the last element in amounts
                                    let currentValue = amounts[amounts.length - 1];

                                    // If there's an increase -> play a sound (fanfare)
                                    if (currentValue > lastCheckValue) {
                                        let audio = new Audio('https://samplelib.com/lib/preview/mp3/sample-3s.mp3');
                                        audio.play();
                                    }
                                    lastCheckValue = currentValue;

                                    // Also update the chart
                                    updateChart(sortedKeys, amounts);
                                }
                            })
                            .catch(error => {
                                console.error('Error fetching JSON data:', error);
                            });
                    }

                    // ---------------------------
                    // updateChart(labels, amounts)
                    // ---------------------------
                    // Replaces the chart's data (labels, aggregated amounts) and updates the chart.
                    function updateChart(labels, amounts) {
                        if (!myChart) return;

                        myChart.data.labels = labels;
                        myChart.data.datasets[0].data = amounts;
                        myChart.update();
                    }

                    // ---------------------------
                    // groupByYearMonth(payments)
                    // ---------------------------
                    // Helper function returning a Map<string, number>
                    // where key = 'YYYY/MM' and value = sum of totalGrossAmount for that month/year
                    function groupByYearMonth(payments) {
                        let map = new Map();
                        payments.forEach(p => {
                            let key = p.year + '/' + p.month;
                            let currentSum = map.has(key) ? map.get(key) : 0;
                            map.set(key, currentSum + p.totalGrossAmount);
                        });
                        return map;
                    }
                </script>");

                html.Append("</head>");
                html.Append("<body>");
                
                html.Append("<h3>Platform income</h3>");
                html.Append("<canvas id='myChart' width='600' height='300' style='max-width:900px; max-height:500px;'></canvas>");
                html.Append("<br>");
                // Build a simple table of PaymentSummary
                html.Append("<table border='1'>");
                html.Append("<thead><tr>");
                html.Append("<th>Year</th>");
                html.Append("<th>Month</th>");
                html.Append("<th>Payment Operator</th>");
                html.Append("<th>Units sold</th>");
                html.Append("<th>Single subscription value</th>");
                html.Append("<th>Total Gross Amount</th>");
                html.Append("</tr></thead>");
                html.Append("<tbody>");

                foreach (var payment in result.Payments)
                {
                    html.Append("<tr>");
                    html.Append($"<td>{payment.Year}</td>");
                    html.Append($"<td>{payment.Month}</td>");
                    html.Append($"<td>{payment.PaymentOperator}</td>");
                    html.Append($"<td>{payment.TotalSuccessCount}</td>");
                    html.Append($"<td>{payment.SubscriptionValue:F2}</td>");
                    html.Append($"<td>{payment.TotalGrossAmount:F2}</td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                // Additional summary info
                html.Append("<h3>Summary:</h3>");
                html.Append($"<p>Total gross income from beginning : {result.TotalIncomGross:F2}</p>");
                html.Append($"<p>Commission Percentage: {result.CommisionPercentage}</p>");
                html.Append($"<p>Vat: {result.Vat}</p>");
                html.Append($"<p>Total Income Net Amount (from beginning): {result.TotalIncomNetAmount:F2}</p>");
                html.Append($"<p>Commission Amount: {result.CommissionAmount:F2}</p>");
                html.Append($"<p>Vat Amount: {result.VatAmount:F2}</p>");

                // Simple form to manipulate parameters
                html.Append("<form method='get'>");
                html.Append("Auth Key: <input type='text' name='authKey' value='" + (authKey ?? "") + "' /> ");
                html.Append("<input type='hidden' name='viewAsHtml' value='true' />");
                html.Append("<br/>Month: <input type='text' name='month' value='" + (month?.ToString() ?? "") + "' />");
                html.Append("<br/>Year: <input type='text' name='year' value='" + (year?.ToString() ?? "") + "' />");
                html.Append("<br/>Commision Percentage: <input type='text' name='commisionPercentage' value='" +
                            commisionPercentage + "' />");
                html.Append("<br/>Vat Percentage: <input type='text' name='vatPercentage' value='" + vatPercentage +
                            "' />");
                html.Append("<br/><input type='submit' value='Submit' />");
                html.Append("</form><br/>");

                html.Append("</body></html>");

                return Content(html.ToString(), "text/html");
            }
            else
            {
                // Return JSON
                return Ok(_paymentSummaryService.Get(authKey, month, year, commisionPercentage, vatPercentage));
            }
        }
    }
}