using ITBees.FAS.Payments.Controllers.Models;
using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using ITBees.UserManager.Interfaces;

namespace ITBees.FAS.Payments.Services;

public class AppleInAppPurchaseService : IAppleInAppPurchaseService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IPaymentSessionService _paymentSessionService;
    private readonly IWriteOnlyRepository<PaymentOperatorLog> _paymentOperatorLogRwRepo;

    public AppleInAppPurchaseService(IAspCurrentUserService aspCurrentUserService, 
        IPaymentSessionService paymentSessionService,
        IWriteOnlyRepository<PaymentOperatorLog> paymentOperatorLogRwRepo)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _paymentSessionService = paymentSessionService;
        _paymentOperatorLogRwRepo = paymentOperatorLogRwRepo;
    }

    public ApplePurchaseConfirmationVm ConfirmPayment(ApplePurchaseIm applePurchaseIm)
    {
        try
        {
            if (string.IsNullOrEmpty(applePurchaseIm.SessionData) == false)
            {
                var ne = new PaymentOperatorLog()
                {
                    Event = "Apple log",
                    JsonEvent = applePurchaseIm.SessionData,
                    Operator = "Apple",
                    Received = DateTime.Now
                };
            
                _paymentOperatorLogRwRepo.InsertData(ne);
            }

            _paymentSessionService.FakeAppleConfirmPayment(applePurchaseIm.PaymentSessionId);

            return new ApplePurchaseConfirmationVm()
            {
                Message = "",
                Success = true
            };
        }
        catch (Exception e)
        {
            return new ApplePurchaseConfirmationVm()
            {
                Message = e.Message,
                Success = false
            };
        }
    }
}