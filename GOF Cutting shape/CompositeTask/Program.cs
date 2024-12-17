namespace CompositeTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lableText = new LabelText("Person");
            var inputField1 = new InputText("Name", "Valentin");
            var inputField2 = new InputText("Age", "34");
            var inputField3 = new InputText("Gender", "M");
            var form1 = new Form("Person data");
            form1.AddComponent(lableText);
            form1.AddComponent(inputField1);
            form1.AddComponent(inputField2);
            form1.AddComponent(inputField3);

            Console.WriteLine(form1.ConvertToString());
        }
    }
}
