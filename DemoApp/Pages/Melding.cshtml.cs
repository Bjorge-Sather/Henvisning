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
            var barn = TestdataGenerator.GetTestdata();
            ViewData["Title"] = (SelectedElementType) ?? "Melding";
        }

        public string MeldingId { get; set; } = "";

        public string SelectedElementType { get; set; } = "";
    }
}
