namespace ITBees.FAS.Payments.Interfaces;

public interface IOrderPackFinalizerService
{
    void CloseSuccessfullyPayedOrderPack(Guid orderPackGuid);
}