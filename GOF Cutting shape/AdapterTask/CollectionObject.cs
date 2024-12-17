namespace AdapterTask;

public class CollectionObject<T> : IElements<string>
{
    public IEnumerable<string> GetElements()
    {
        var colors = new List<string>
        { "Green", "Blush", "Yellow",  "Red", "Orange", "Burgandy","Purple",
           "White", "Black", "Blue" ,"Bronze"};
        return colors;
    }
}
