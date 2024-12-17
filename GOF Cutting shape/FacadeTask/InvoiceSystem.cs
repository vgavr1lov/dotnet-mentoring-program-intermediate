namespace FacadeTask;

public class InvoiceSystem : IInvoiceSystem
{
    public void SendInvoice(Invoice invoice)
    {
        Console.WriteLine($"invoice has been sent");
    }
}
