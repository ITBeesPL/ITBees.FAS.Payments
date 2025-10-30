namespace ITBees.FAS.Payments.Interfaces;

public sealed class EcrSettlementResult
{
    public bool Success { get; set; }
    public string AuthCode { get; set; }
    public string CombinedAscii { get; set; }
    public string CombinedTlvHex { get; set; }
    public IReadOnlyList<string> Frames { get; set; }
}