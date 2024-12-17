namespace CompositeTask;

public class InputText: IXmlElement
{
    readonly string _name;
    readonly string _value;
    public InputText(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public string ConvertToString()
    {
        return $"<inputText name='{_name}' value='{_value}'/>";
    }

}
