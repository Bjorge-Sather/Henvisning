using System.Xml.Schema;
namespace DemoApp.Models.ViewModels
{
    public class PropertyRendererModel(string xPath, XmlSchemaAnnotated prop, object? value)
    {
        public string XPath { get; } = $"{xPath}/{XsdUtils.GetName(prop)}";
        public XmlSchemaAnnotated Prop { get; } = prop;
        public object? Value { get; } = value;

        public string? GetCaption(bool fallbackToName)
        {
            return XsdUtils.GetCaption(Prop, fallbackToName);
        }

        public DateTime Start { get; set; } = DateTime.Now.Date;

        public string GetDescription()
        {
            if (Prop == null) return
                    "<Prop=null>";
            return XsdUtils.GetAppInfoValue(Prop, "veiledning");
        }

        public string GetId()
        {
            if (Prop is XmlSchemaElement || Prop is XmlSchemaAttribute)
                return $"{XPath}";
            return "---";
        }

        public int GetMinLength()
        {
            return XsdUtils.GetMinLength(Prop);
        }
        public int GetMaxLength()
        {
            return XsdUtils.GetMaxLength(Prop);
        }

    }
}
