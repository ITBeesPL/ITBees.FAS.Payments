using ITBees.FAS.Payments.Controllers.Operator;

namespace ITBees.FAS.Payments.Services.Operator;

public interface IPaymentSummaryService
{
    PaymentSummaryVm Get(string? authKey, int? month, int? year,int commisionPercentage, int vatPercentage);
}