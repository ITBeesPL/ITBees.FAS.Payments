namespace ITBees.FAS.Payments.Services;

public class BillingPeriod
{
    public static FasBillingPeriod GetBillingPeriod(int interval)
    {
        switch (interval)
        {
            case 0:
                return FasBillingPeriod.Daily;
            case 1:
                return FasBillingPeriod.Monthly;
            case 2:
                return FasBillingPeriod.Weekly;
            case 3:
                return FasBillingPeriod.Every3Months;
            case 6:
                return FasBillingPeriod.Every6Months;
            case 12:
                return FasBillingPeriod.Yearly;
            default:
                throw new ArgumentOutOfRangeException("Value of Billing  period must be equals :0 " +
                                                      "(daily), 1 (monthly), 2 (weekly), 3 (quarterly), 6 (half year), 12 (yearly)");
        }
    }
}