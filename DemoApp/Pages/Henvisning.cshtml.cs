using DemoApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApp.Pages
{
    public class HenvisningModel : PageModel
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
        }

        public string MeldingId { get; set; } = "";

        public string SelectedElementType { get; set; } = "";
    }
}
