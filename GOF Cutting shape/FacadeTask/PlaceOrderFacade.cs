namespace FacadeTask;

public class PlaceOrderFacade
{
    IProductCatalog _productCatalog;
    IPaymentSystem _paymentSystem;
    IInvoiceSystem _invoiceSystem;
    public PlaceOrderFacade(IProductCatalog productCatalog, IPaymentSystem paymentSystem, IInvoiceSystem invoiceSystem)
    {
        _productCatalog = productCatalog;
        _paymentSystem = paymentSystem;
        _invoiceSystem = invoiceSystem;
    }
    public void PlaceOrder(string productId, int quantity, string email)
    {
        var product = _productCatalog.GetProductDetails(productId);
        if ( product == null)
        {
            Console.WriteLine($"Product with Id {productId} is not found!");
            return;
        }

        Console.WriteLine($"Product with Id {productId} is found!");

        var payment = new Payment();
        var isPaymentCompleted = _paymentSystem.MakePayment(payment);
        if (!isPaymentCompleted)
        {
            Console.WriteLine($"Payment is failed!");
            return;
        }

        Console.WriteLine($"Payment is completed!");

        var invoice = new Invoice();
        _invoiceSystem.SendInvoice(invoice);
    }
}
