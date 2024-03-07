using System.Xml.Schema;
namespace DemoApp.Models.ViewModels
{
    public class PropertyRendererModel(XmlSchemaAnnotated prop, object? value)
    {
        public XmlSchemaAnnotated Prop { get; } = prop;
        public object? Value { get; } = value;

        public string? GetCaption(bool fallbackToName)
        {
            if (Prop == null) return
                    "<Prop=null>";
            var result = DataFactory.GetAppInfoValue(Prop, "ledetekst");
            if (result == "" && fallbackToName)
            {
                if (Prop is XmlSchemaElement element)
                    return element.Name;
                else if (Prop is XmlSchemaAttribute attribute)
                    return attribute.Name;
            }
            return result;
        }

        public string GetDescription()
        {
            if (Prop == null) return
                    "<Prop=null>";
            return DataFactory.GetAppInfoValue(Prop, "veiledning");
        }

        public string GetId()
        {
            if (Prop is XmlSchemaElement element)
                return element.QualifiedName.Name;
            if (Prop is XmlSchemaAttribute attribute)
                return attribute.QualifiedName.Name;
            return "---";
        }

        public int GetMinLength()
        {
            return DataFactory.GetMinLength(Prop);
        }
        public int GetMaxLength()
        {
            return DataFactory.GetMaxLength(Prop);
        }

    }
}
