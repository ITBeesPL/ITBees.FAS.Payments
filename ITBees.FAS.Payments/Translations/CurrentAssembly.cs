using ITBees.Translations.Interfaces;
using ITBees.UserManager.Translations;

namespace ITBees.FAS.Payments.Translations;

public class CurrentAssembly
{
    public static List<ITranslate> GetTranslationClasses()
    {
        return [new ApplySubscriptionPlan()];
    }
}