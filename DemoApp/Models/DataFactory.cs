using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace DemoApp.Models
{
    public static class DataFactory
    {
        private const string DATA_PATH = @"\App_Data";
        private const string DEFS_PATH = @"\Defs";

        public static List<XmlSchema> XsdSchemasWithRootElement { get; set; } = [];

        private static string GetRootDirectory()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        public static void LoadXsds()
        {
            string DefsPath = $"{GetRootDirectory()}{DEFS_PATH}";
            var files = new DirectoryInfo(DefsPath).GetFiles(@"*.xsd", SearchOption.AllDirectories);
            XmlSchemaSet schemaSet = new();
            foreach (var file in files)
            {
                XmlSchema schema = XmlSchema.Read(XmlReader.Create(file.FullName), null)
                        ?? throw new Exception($"Kan ikke laste {file.Name}");
                schemaSet.Add(schema);
            }
            schemaSet.Compile();
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                if (schema.Elements.Count > 0)
                {
                    XsdSchemasWithRootElement.Add(schema);
                }
            }

        }

        public static List<XmlSchemaAnnotated> GetXsdElements(XmlSchemaAnnotated? prop, bool flat = false)
        {
            List<XmlSchemaAnnotated> list = [];
            if (prop != null)
                GetXsdElements(prop, flat, ref list);
            return list;
        }


        public static void GetXsdElements(XmlSchemaAnnotated prop, bool flat, ref List<XmlSchemaAnnotated> list)
        {
            list.Add(prop);
            if (flat && prop is XmlSchemaElement element)
                if (element.ElementType is XmlSchemaComplexType complexElement && flat)
                {
                    if (complexElement.Particle is XmlSchemaSequence sequenceElement)
                    {
                        foreach (XmlSchemaElement child in sequenceElement.Items)
                        {
                            GetXsdElements(child, false, ref list);
                        }
                    }
                }
        }

        public static List<XmlSchemaAnnotated> GetXsdChildren(XmlSchemaAnnotated prop)
        {
            List<XmlSchemaAnnotated> list = [];
            if (prop is XmlSchemaElement element && element.ElementType is XmlSchemaComplexType complexElement)
            {
                if (complexElement.Particle is XmlSchemaSequence sequenceElement)
                {
                    foreach (XmlSchemaElement child in sequenceElement.Items)
                    {
                        list.Add(child);
                    }
                }
            }
            return list;
        }

        public static string GetControlNameForProperty(XmlSchemaAnnotated prop)
        {
            string controlName;
            XmlSchemaDatatype datatype = null;
            var simpleType = GetSimpleType(prop);
            if (prop is XmlSchemaElement element)
            {
                if (element.ElementSchemaType is XmlSchemaComplexType)
                    return "Gruppe";
                if (GetIsEnumType(simpleType))
                {
                    if (element.MaxOccurs > 1)
                    {
                        return "MultipleChoice";
                    }
                    return "SelectOne";
                }

                datatype = element.ElementSchemaType.Datatype;
            }
            else if (prop is XmlSchemaAttribute attribute)
            {
                datatype = simpleType.Datatype;
            }

            controlName = datatype.TypeCode switch
            {
                XmlTypeCode.Boolean => "Checkbox",
                XmlTypeCode.Date => "Dato",
                _ => "Tekst"
            };
            return controlName;
        }

        public static Dictionary<string, string> GetEnumValues(XmlSchemaAnnotated prop)
        {
            var simpleType = GetSimpleType(prop);
            if (simpleType != null)
            {
                return GetEnumValues(simpleType);
            }
            throw new Exception($"{prop} is not an Enumeration type");
        }
        public static Dictionary<string, string> GetEnumValues(XmlSchemaSimpleType simpleType)
        {
            var result = new Dictionary<string, string>();
            if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
            {
                foreach (XmlSchemaEnumerationFacet facet in restriction.Facets.OfType<XmlSchemaEnumerationFacet>())
                {
                    result[facet.Value] = facet.Value;
                }
            }
            return result;
        }
        public static bool GetIsEnumType(XmlSchemaSimpleType? simpleType)
        {
            if (simpleType?.Content is XmlSchemaSimpleTypeRestriction restriction)
            {
                foreach (var facet in restriction.Facets.OfType<XmlSchemaEnumerationFacet>())
                {
                    return true;
                }
            }
            return false;
        }

        public static XmlSchemaSimpleType? GetSimpleType(XmlSchemaAnnotated prop)
        {
            if (prop is XmlSchemaElement element && element.ElementType is XmlSchemaSimpleType simpleType)
            {
                return simpleType;
            }
            else if (prop is XmlSchemaAttribute attribute && attribute.AttributeType is XmlSchemaSimpleType attrSimpleType)
            {
                return attrSimpleType;
            }
            return null;
        }

        public static int GetMinLength(XmlSchemaAnnotated prop)
        {
            var simpleType = GetSimpleType(prop);
            if (simpleType != null)
            {
                if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
                {
                    foreach (var facet in restriction.Facets.OfType<XmlSchemaMinLengthFacet>())
                    {
                        if (int.TryParse(facet.Value, out int len))
                            return len;
                    }
                }
            }
            return 0;
        }
        public static int GetMaxLength(XmlSchemaAnnotated prop)
        {
            var simpleType = GetSimpleType(prop);
            if (simpleType != null)
            {
                if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
                {
                    foreach (var facet in restriction.Facets.OfType<XmlSchemaMaxLengthFacet>())
                    {
                        if (int.TryParse(facet.Value, out int len))
                            return len;
                    }
                }
            }
            return 0;
        }
        internal static string GetMarkupText(XmlNode?[]? markup)
        {
            if (markup != null && markup.Length > 0)
            {
                StringBuilder sb = new();
                foreach (var node in markup)
                {
                    if (node != null)
                    {
                        sb.Append(node.Value?.Trim() ?? "");
                    }
                }
                return sb.ToString();
            }
            return "";
        }
    }
}
