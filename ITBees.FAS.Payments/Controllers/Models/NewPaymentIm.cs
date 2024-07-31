namespace ITBees.FAS.Payments.Controllers.Models;

public class NewPaymentIm
{
    public Guid CompanyGuid { get; set; }
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public string ProductName { get; set; }
    public Guid CurrentUserGuid { get; set; }
    public Guid PlatformSubscriptionPlanGuid { get; set; }
    public string Currency { get; set; }
    /// <summary>
    /// use month, year, etc
    /// </summary>
    public string Interval { get; set; }
    /// <summary>
    /// maximum number of repetitions is 3 years
    /// </summary>
    public int IntervalCount { get; set; }
}