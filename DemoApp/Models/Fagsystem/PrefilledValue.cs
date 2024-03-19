namespace DemoApp.Models.Fagsystem
{
    public class PrefilledValue(string xpath, string value, bool openForEdit)
    {
        public string Xpath { get; } = xpath;
        public string Value { get; } = value;
        public bool OpenForEdit { get; } = openForEdit;
    }
}
