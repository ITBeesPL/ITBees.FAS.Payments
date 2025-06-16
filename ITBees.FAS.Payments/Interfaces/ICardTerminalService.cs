using ITBees.FAS.Payments.Setup;

namespace ITBees.FAS.Payments.Interfaces;

public interface ICardTerminalService
{
    event Action<TransactionResult> TransactionSucceeded;
    event Action<TransactionResult> TransactionFailed;

    Task<TransactionResult> StartTransactionAsync(string type, decimal amount);
}