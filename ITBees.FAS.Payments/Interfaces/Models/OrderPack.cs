using ITBees.Models.Users;

namespace ITBees.FAS.Payments.Interfaces.Models;

public class OrderPack
{
    public Guid Guid { get; set; }
    public List<OrderElement> OrderElements { get; set; } = new();
    public UserAccount CreatedBy { get; set; }
    public Guid CreatedByGuid { get; set; }
    public DateTime Created { get; set; }
}