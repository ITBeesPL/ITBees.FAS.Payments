using ITBees.Translations.Interfaces;

namespace ITBees.FAS.Payments.Translations;

public class ApplySubscriptionPlan : ITranslate
{
    public class Errors : ITranslate
    {
        public static readonly string TrialPlanAlreadyUsed =
            "The free trial can be activated only once – it has already been used on this account.";
    }
}