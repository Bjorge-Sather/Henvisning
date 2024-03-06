namespace DemoApp.Models
{
    public static class Utils
    {
        public static void GetRequestParams(HttpRequest req, out Dictionary<string, string> queryParams)
        {
            queryParams = new Dictionary<string, string>();
            foreach (var kvp in req.Query)
            {
                queryParams[kvp.Key] = kvp.Value;
            }
            if (req.HasFormContentType)
            {
                foreach (var kvp in req.Form)
                {
                    queryParams[kvp.Key] = kvp.Value;
                }
            }
        }

        public static string GetRequestValue(Dictionary<string, string> RequestParams, string param)
        {
            if (RequestParams.ContainsKey(param))
                return RequestParams[param];
            return "";
        }
    }
}
