using ITBees.FAS.Payments.Setup;

namespace ITBees.FAS.Payments.Interfaces;

public interface ICardTerminalService
{
    event Action<TransactionResult> TransactionSucceeded;
    event Action<TransactionResult> TransactionFailed;

    Task<TransactionResult> StartTransactionAsync(CashlessTransactionType type, decimal amount, string prefixComment);
    Task<TransactionResult> StartRefundAsync(decimal amount, string prefixComment);
    Task<TransactionResult> StartReversalAsync(decimal amount, string prefixComment, string rrnHex12 = null);
    Task<bool> CloseDayAsync(string prefixComment, string messageId = "2841311", string tlvHex = null);
    Task<bool> RequestTransactionReportAsync(string prefixComment, string messageId = "2841211", string tlvHex = null);
    Task SendHelloExtendedAsync(string companyName = null, string ecrVersion = null, string protocolVersionNnnmmpp = "0047511");
    Task<EcrReportResult> RequestPreviewCumulativeReportAsync(int bParam, int? profileId = null,
        int timeoutMs = 20000);
}