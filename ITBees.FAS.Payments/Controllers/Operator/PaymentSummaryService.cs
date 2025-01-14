using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.FAS.Payments.Services;
using ITBees.FAS.Payments.Services.Operator;
using ITBees.Interfaces.Platforms;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Controllers.Operator;

public class PaymentSummaryService : IPaymentSummaryService
{
    private readonly IReadOnlyRepository<PaymentSession> _paymentSessionRepository;
    private readonly IAccessChecker _accessChecker;

    public PaymentSummaryService(
        IReadOnlyRepository<PaymentSession> paymentSessionRepository,
        IAccessChecker accessChecker)
    {
        _paymentSessionRepository = paymentSessionRepository;
        _accessChecker = accessChecker;
    }
    
    public PaymentSummaryVm Get(string? authKey, int? month, int? year, int commisionPercentage, int vatPercentage)
    {
        _accessChecker.CheckAccess(authKey);

        var data = _paymentSessionRepository.GetDataQueryable(
                x => x.Success && x.FinishedDate.HasValue, 
                x => x.InvoiceData.SubscriptionPlan)
            .GroupBy(x => new { x.FinishedDate.Value.Year, x.FinishedDate.Value.Month, x.PaymentOperator, x.InvoiceData.SubscriptionPlan.Value })
            .Select(g => new PaymentSummaryElementVm
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                PaymentOperator = g.Key.PaymentOperator,
                TotalSuccessCount = g.Count(),
                TotalGrossAmount = g.Sum(x => x.InvoiceData.SubscriptionPlan.Value),
                TotalInvoicesCreated = g.Count(x=>x.InvoiceCreated),
                SubscriptionValue = g.Key.Value
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.PaymentOperator)
            .ToList();

        var totalGrossAmount = data.Sum(x => x.TotalGrossAmount);

        var totalNetAmount = totalGrossAmount / (1 + vatPercentage / 100.0m);

        var commissionAmount = totalNetAmount * commisionPercentage / 100.0m;

        var vatAmount = totalNetAmount * vatPercentage / 100.0m;

        return new PaymentSummaryVm()
        {
            Payments = data,
            TotalIncomGross = totalGrossAmount,
            TotalIncomNetAmount = totalNetAmount,
            CommisionPercentage = $"{commisionPercentage} %",
            CommissionAmount = commissionAmount,
            Vat = $"{vatPercentage} %",
            VatAmount = vatAmount
        };
    }

}