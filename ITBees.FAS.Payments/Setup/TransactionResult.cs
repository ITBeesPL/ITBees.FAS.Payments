namespace ITBees.FAS.Payments.Setup;

public class TransactionResult
{
    public bool Success { get; set; }
    public decimal Amount { get; set; }
    public string CardSuffix { get; set; }
    public string Message { get; set; }
    public string Stan { get; set; }
    public string AuthCode { get; set; }
    public string RetrievalReferenceNumber { get; set; }
    public string? TransactionType { get; set; }
}