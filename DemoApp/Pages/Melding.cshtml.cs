using DemoApp.Models;
using DemoApp.Models.Fagsystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApp.Pages
{
    public class MeldingModel : PageModel
    {
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
            Utils.GetRequestParams(Request, out Dictionary<string, string> queryParams);
            SelectedElementType = Utils.GetRequestValue(queryParams, "selectedSchemaName");
            ViewData["Title"] = (SelectedElementType) ?? "Melding";
            PrefillValues = TestdataGenerator.GetPrefillValues(Request);
        }

        public List<PrefilledValue> PrefillValues { get; set; }
        public string MeldingId { get; set; } = "";

        public string SelectedElementType { get; set; } = "";
    }
}
