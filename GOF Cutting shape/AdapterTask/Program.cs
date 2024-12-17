namespace AdapterTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var collectionObject = new CollectionObject<string>();
            var containerAdapter = new ContainerAdapter<string>(collectionObject);
            var printer = new Printer();
            printer.Print(containerAdapter);
        }
    }
}
