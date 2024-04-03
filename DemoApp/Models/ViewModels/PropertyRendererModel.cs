using DemoApp.Models.Fagsystem;
using System.Xml.Schema;
namespace DemoApp.Models.ViewModels
{
    public class PropertyRendererModel(string xPath, XmlSchemaAnnotated prop, List<PrefilledValue> values, XmlSchemaAnnotated skipProp = null)
    {
        public string XPath { get; } = $"{xPath}/{XsdUtils.GetName(prop)}";
        public XmlSchemaAnnotated Prop { get; } = prop;
        public List<PrefilledValue> Values { get; } = values;

        public XmlSchemaAnnotated? SkipProp { get; } = skipProp;

        public string? GetCaption(bool fallbackToName)
        {
            return XsdUtils.GetCaption(Prop, fallbackToName);
        }

        public bool Mandatory
        {
            get
            {
                return XsdUtils.PropIsMandatory(Prop);
            }
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

        public XmlSchemaAnnotated GetChildTypeDefinition(string path)
        {
            var child = XsdUtils.GetChildByPath(Prop, path);
            if (child != null)
                return XsdUtils.GetSimpleType(child);
            return null;
        }

    }
}
