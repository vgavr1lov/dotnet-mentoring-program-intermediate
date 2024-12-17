namespace FacadeTask
{
    public interface IPaymentSystem
    {
        bool MakePayment(Payment payment);
    }
}