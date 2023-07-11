namespace GBC_Bids_Group.Helpers
{
    public static class MfaCookiesHelper
    {
        public static void SetCode(HttpContext httpContext, string code)
        {
            httpContext.Response.Cookies.Append("Code", code);
        }
        public static string GetCode(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue("Code", out string code))
            {
                return code;
            }
            else
            {
                return null;
            }
        }
        public static void ClearCode(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("Code");
        }
    }
}
