using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DemoApp.Models
{
    public static class DataFactory
    {
        private const string DATA_PATH = @"\App_Data";
        private const string DEFS_PATH = @"\Defs";

        public static List<XmlSchema> XsdSchemasWithRootElement { get; set; } = [];

        public static Kodelister? kodelister { get; set; } = null;

        private static string? GetRootDirectory()
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
            LoadKodelister();

        }

        public static void LoadKodelister()
        {
            string defsPath = $"{GetRootDirectory()}{DEFS_PATH}";
            var serializer = new XmlSerializer(typeof(Kodelister));
            using (var reader = new StreamReader($"{defsPath}\\Bufdir-kodelister_0.1.0.xml"))
            {
                kodelister = serializer.Deserialize(reader) as Kodelister;
            }
            InitKodelister();
        }

        private static void InitKodelister()
        {
            if (kodelister == null)
                throw new Exception("Kodelister er ikke lastet");
            if (kodelister.kodelister == null)
                throw new Exception("Kodelister er lastet, men er tom");
            foreach (Kodeliste liste in kodelister.kodelister)
            {
                if (!string.IsNullOrEmpty(liste.utdragfra))
                {
                    var kildeListe = GetKildeKodeliste(liste.utdragfra);
                    if (liste.koder != null)
                    {
                        foreach (var kode in liste.koder)
                        {
                            var kildeKode = kildeListe.koder?.FirstOrDefault(k => k.verdi == kode.verdi)
                                ?? throw new Exception($"Finner ikke kode {kode.verdi} i kildeliste {kildeListe.navn}");
                            kode.tekst = kildeKode.tekst;
                        }
                    }
                }
            }
        }

        public static Kodeliste GetKildeKodeliste(string kilde, int level = 0)
        {
            if (level > 10)
            {
                throw new Exception($"Søk etter kildeliste {kilde} går i loop");
            }
            var liste = kodelister?.kodelister?.FirstOrDefault(k => k.navn == kilde);
            if (!string.IsNullOrEmpty(liste?.utdragfra))
                liste = GetKildeKodeliste(liste.utdragfra, level + 1);
            if (liste == null)
                throw new Exception($"Finner ikke kildeliste {kilde}");
            return liste;
        }

        public static Kodeliste GetKodeliste(XmlSchemaAnnotated prop)
        {
            var simpleType = GetSimpleType(prop)
                ?? throw new Exception($"Finner ikke type {prop}");
            return GetKodeliste(simpleType.Name!);
        }

        public static Kodeliste GetKodeliste(string navn)
        {
            var result = kodelister?.kodelister?.FirstOrDefault(k => k.navn == navn)
                ?? throw new Exception($"Kodeliste {navn} ikke funnet");
            return result;
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
                if (element.ElementSchemaType is XmlSchemaComplexType complexElement && flat)
                {
                    if (complexElement.Particle is XmlSchemaSequence sequenceElement)
                    {
                        foreach (XmlSchemaElement child in sequenceElement.Items.Cast<XmlSchemaElement>())
                        {
                            GetXsdElements(child, false, ref list);
                        }
                    }
                }
        }

        public static List<XmlSchemaAnnotated> GetXsdChildren(XmlSchemaAnnotated prop)
        {
            List<XmlSchemaAnnotated> list = [];
            if (prop is XmlSchemaElement element && element.ElementSchemaType is XmlSchemaComplexType complexElement)
            {
                if (complexElement.Particle is XmlSchemaSequence sequenceElement)
                {
                    foreach (XmlSchemaElement child in sequenceElement.Items.Cast<XmlSchemaElement>())
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
            XmlSchemaDatatype? datatype = null;
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
                if (element?.ElementSchemaType?.Datatype == null)
                    throw new Exception("Datatype == null");
                datatype = element.ElementSchemaType.Datatype;
            }
            else if (prop is XmlSchemaAttribute)
            {
                datatype = simpleType?.Datatype ??
                    throw new Exception("simpleType?.Datatype == null");
            }

            controlName = datatype?.TypeCode switch
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
                    if (facet.Value != null)
                        result[facet.Value] = facet.Value;
                }
            }
            return result;
        }
        public static bool GetIsEnumType(XmlSchemaSimpleType? simpleType)
        {
            if (simpleType?.Content is XmlSchemaSimpleTypeRestriction restriction)
            {
                if (restriction.Facets.OfType<XmlSchemaEnumerationFacet>().Any())
                {
                    return true;
                }
            }
            return false;
        }

        public static XmlSchemaSimpleType? GetSimpleType(XmlSchemaAnnotated prop)
        {
            if (prop is XmlSchemaElement element && element.ElementSchemaType is XmlSchemaSimpleType simpleType)
            {
                return simpleType;
            }
            else if (prop is XmlSchemaAttribute attribute && attribute.AttributeSchemaType is XmlSchemaSimpleType attrSimpleType)
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

        private static string GetAppInfoElement(XmlNode?[]? markup, string elementName)
        {
            if (markup?.Length >= 1)
            {
                for (int i = 0; i < markup.Length; i++)
                {
                    if (markup[i]?.Name == elementName)
                    {
                        return markup[i]!.InnerText.Trim();
                    }
                }
            }
            return "";
        }


        public static string GetAppInfoValue(XmlSchemaAnnotated xsdType, string appInfoElement, bool recurse = true)
        {
            if (xsdType.Annotation != null)
            {
                foreach (var item in xsdType.Annotation.Items)
                {
                    if (item is XmlSchemaAppInfo appInfo)
                    {
                        return GetAppInfoElement(appInfo.Markup, appInfoElement);
                    }
                }
            }
            if (recurse)
            {
                if (xsdType is XmlSchemaElement element)
                {
                    return GetAppInfoValue(element.ElementSchemaType, appInfoElement, false);
                }
                else if (xsdType is XmlSchemaAttribute attribute)
                {
                    return GetAppInfoValue(attribute.AttributeSchemaType, appInfoElement, false);
                }
            }
            return "";
        }
    }
}
