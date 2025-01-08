namespace ITBees.FAS.Payments.Services;

public interface IAccessChecker
{
    /// <summary>
    /// Checks access based on authKey, and current session user (is platform operator?) and throws Unauthorized access attempt if not allowed
    /// </summary>
    /// <param name="authKey"></param>
    void CheckAccess(string? authKey);
}