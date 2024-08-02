using ITBees.FAS.Payments.Interfaces;
using ITBees.FAS.Payments.Interfaces.Models;
using ITBees.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace ITBees.FAS.Payments.Services;

public class PaymentDbLoggerService : IPaymentDbLoggerService
{
    private readonly IWriteOnlyRepository<PaymentOperatorLog> _paymentOperatorLogRwRepo;
    private readonly ILogger<PaymentDbLoggerService> _logger;

    public PaymentDbLoggerService(IWriteOnlyRepository<PaymentOperatorLog> paymentOperatorLogRwRepo, ILogger<PaymentDbLoggerService> logger)
    {
        _paymentOperatorLogRwRepo = paymentOperatorLogRwRepo;
        _logger = logger;
    }


    public void Log(PaymentOperatorLog log)
    {
        try
        {
            _paymentOperatorLogRwRepo.InsertData(log);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, log.Event);
        }
    }
}