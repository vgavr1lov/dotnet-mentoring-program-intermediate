namespace CompositeTask;

public class LabelText: IXmlElement
{
    readonly string _value;
    public LabelText(string value)
    {
        _value = value;
    }

    public string ConvertToString()
    {
        return $"<label value='{_value}'/>";
    }

}
