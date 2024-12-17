using System.Text;

namespace CompositeTask;

public class Form : IXmlElement
{
    readonly string _name;
    readonly List<IXmlElement> _children = new();

    public Form(string name)
    {
        _name = name;
    }

    public void AddComponent(IXmlElement xmlElement)
    {
        _children.Add(xmlElement);
    }

    public string ConvertToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"<form name='{_name}'>");
        _children.ForEach(child => stringBuilder.Append(child.ConvertToString()));
        stringBuilder.Append("</form>");

        return stringBuilder.ToString();
    }
}
