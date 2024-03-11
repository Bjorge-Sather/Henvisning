using DemoApp.Models;
using DemoApp.Models.Fagsystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApp.Pages
{
    public class BarnModel : PageModel
    {
        public List<Barn> BarnListe { get; private set; }

        public Barn? selectedBarn { get; set; }

        public void OnGet()
        {
            Init();
        }

        public void OnPost()
        {
            Init();
        }

        public void Init()
        {
            this.BarnListe = TestdataGenerator.GetTestdata();
            Utils.GetRequestParams(Request, out Dictionary<string, string> queryParams);
            string barnId = Utils.GetRequestValue(queryParams, "barn_id");
            if (!string.IsNullOrEmpty(barnId))
                selectedBarn = BarnListe.FirstOrDefault(b => b.Id.ToString() == barnId);

        }
    }
}
