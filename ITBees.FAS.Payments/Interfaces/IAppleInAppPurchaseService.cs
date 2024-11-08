using ITBees.FAS.Payments.Controllers;
using ITBees.FAS.Payments.Controllers.Models;

namespace ITBees.FAS.Payments.Interfaces;

public interface IAppleInAppPurchaseService
{
    ApplePurchaseConfirmationVm ConfirmPayment(ApplePurchaseIm applePurchaseIm);
}