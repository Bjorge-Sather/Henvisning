using System.Xml.Serialization;

namespace DemoApp.Models
{
    [Serializable]
    [XmlRoot(ElementName = "kodelister")]//, Namespace = "kodelister.bufdir.no", DataType = "string", IsNullable = true)]
    public class Kodelister
    {
        [XmlElement(ElementName = "kodeliste")]
        public Kodeliste[]? kodelister { get; set; }
    }
}
