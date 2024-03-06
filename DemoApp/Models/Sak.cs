namespace DemoApp.Models
{
    public class Sak
    {
        public Barn? Barn { get; set; }
        public List<string> _meldinger { get; set; } = [];
        public List<Melding> Meldinger { get; set; } = [];
    }
}
