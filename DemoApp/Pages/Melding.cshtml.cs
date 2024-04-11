using DemoApp.Models;
using DemoApp.Models.Fagsystem;
using Microsoft.AspNetCore.Html;
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
            if (queryParams.ContainsKey("LagHTML"))
            {
                // Lagre til XML
                // Transformere XML v/XSLT
                // Vise melding
            }
            else if (queryParams.ContainsKey("Send"))
            {
                // Lagre til XML
                // Redirect til 
            }
            SelectedElementType = Utils.GetRequestValue(queryParams, "selectedSchemaName");
            ViewData["Title"] = (SelectedElementType) ?? "Melding";
            PrefillValues = TestdataGenerator.GetPrefillValues(Request);
        }

        public HtmlString TransformedDocument { get; set; }

        public List<PrefilledValue>? PrefillValues { get; set; }
        public string MeldingId { get; set; } = "";

        public string SelectedElementType { get; set; } = "";
    }
}
