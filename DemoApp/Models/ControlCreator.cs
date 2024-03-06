using System.Xml.Schema;

namespace DemoApp.Models
{
    public static class ControlCreator
    {
        const string CSS_ELEMENT_BESKRIVELSE = "element_beskrivelse";
        const string CSS_ELEMENT_LEDETEKST = "element_ledetekst";
        public static string CreateControlsFor(XmlSchemaElement element, string cssClass, string id)
        {
            string elementName = element.Name!;
            string description = "";
            if (element.Annotation != null)
            {
                foreach (var item in element.Annotation.Items)
                {
                }
            }
            else
            {
            }
            return $"<span class='{CSS_ELEMENT_LEDETEKST}'>{elementName}</span><span class='{CSS_ELEMENT_BESKRIVELSE}'>{description}</span>";
        }
    }
}
