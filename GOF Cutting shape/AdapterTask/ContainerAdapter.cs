namespace AdapterTask;

public class ContainerAdapter<T> : IContainer<T>
{
    IEnumerable<T> _elements;
    public IEnumerable<T> Items => _elements;

    public int Count => Items.Count();

    public ContainerAdapter(IElements<T> elements)
    {
        _elements = elements.GetElements();
    }
}