using System.Xml;
using System.Xml.Schema;
namespace DemoApp.Models.ViewModels
{
    public class PropertyRendererModel
    {
        public PropertyRendererModel(XmlSchemaAnnotated prop, object? value)
        {
            this.Prop = prop;
            this.Value = value;
        }

        public XmlSchemaAnnotated? Prop { get; }
        public object? Value { get; }

        public string GetCaption()
        {
            if (Prop == null) return
                    "<Prop=null>";
            if (Prop.Annotation != null)
            {
                foreach (var item in Prop.Annotation.Items)
                {
                    if (item is XmlSchemaAppInfo appInfo)
                    {
                        return GetAppInfoElement(appInfo.Markup, "ledetekst");
                    }
                }
            }
            if (Prop is XmlSchemaElement element)
                return element.Name;
            else if (Prop is XmlSchemaAttribute attribute)
                return attribute.Name;
            return "(no name)";
        }

        private string GetAppInfoElement(XmlNode?[]? markup, string elementName)
        {
            if (markup?.Length >= 1)
            {
                for (int i = 0; i < markup.Length; i++)
                {
                    if (markup[i].Name == elementName)
                    {
                        return markup[i].InnerText.Trim();
                    }
                }
            }
            return "";
        }

        public string GetDescription()
        {
            if (Prop == null) return
                    "<Prop=null>";
            if (Prop.Annotation != null)
            {
                foreach (var item in Prop.Annotation.Items)
                {
                    if (item is XmlSchemaAppInfo appInfo)
                    {
                        return GetAppInfoElement(appInfo.Markup, "veiledning");
                    }
                    else if (item is XmlSchemaDocumentation doc)
                    {
                        return DataFactory.GetMarkupText(doc.Markup);
                    }
                }
            }

            return "";
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
