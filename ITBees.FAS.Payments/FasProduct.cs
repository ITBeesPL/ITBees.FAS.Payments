﻿namespace ITBees.FAS.Payments;

public class FasProduct
{
    public Guid Guid { get; set; }
    public string PaymentTitleOrProductName { get; set; }
    public long Quantity { get; set; }
    public string Currency { get; set; }
    public FasBillingPeriod BillingPeriod { get; set; }
    public decimal Price { get; set; }
    /// <summary>
    /// use month, year, etc
    /// </summary>
    public string Interval { get; set; }
    /// <summary>
    /// maximum number of repetitions is 3 years
    /// </summary>
    public int IntervalCount { get; set; }
}