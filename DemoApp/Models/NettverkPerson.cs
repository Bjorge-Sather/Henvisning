namespace DemoApp.Models
{
    public enum NettverkRelasjonType
    {
        Mor,
        Far,
        Annet
    }
    public class NettverkPerson
    {
        public int Id { get; set; }
        public FREG_Person fREG_Person { get; set; }
        public NettverkRelasjonType NettverkRelasjon { get; set; }
    }
}
