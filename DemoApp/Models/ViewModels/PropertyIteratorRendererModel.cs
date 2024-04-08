using DemoApp.Models.Fagsystem;
using System.Xml.Schema;
namespace DemoApp.Models.ViewModels
{
    public class PropertyIteratorRendererModel(string xPath, XmlSchemaAnnotated prop, List<PrefilledValue> values, string iteratorElement) : PropertyRendererModel(xPath, prop, values)
    {
        public string Iterator => iteratorElement;
        public XmlSchemaAnnotated? GetIterateTypeDefinition()
        {
            var child = XsdUtils.GetChildByPath(Prop, Iterator);
            if (child != null)
                return XsdUtils.GetSimpleType(child);
            return null;
        }
    }
}
