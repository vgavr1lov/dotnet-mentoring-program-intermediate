namespace FacadeTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var productCatalog = new ProductCatalog();
            var paymentSystem = new PaymentSystem();
            var invoiceSystem = new InvoiceSystem();
            var placeOrderFacade = new PlaceOrderFacade(productCatalog, paymentSystem, invoiceSystem);
            placeOrderFacade.PlaceOrder("1", 10, "test@email.com");
        }
    }
}
