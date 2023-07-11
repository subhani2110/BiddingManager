namespace GBC_Bids_Group.Helpers
{
    using Microsoft.AspNetCore.Http;

    public class SessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string username, string role, int userId)
        {
            _httpContextAccessor.HttpContext.Session.SetString("Username", username);
            _httpContextAccessor.HttpContext.Session.SetString("UserRole", role);
            _httpContextAccessor.HttpContext.Session.SetString("UserId", userId.ToString());
        }

        public bool IsAuthenticatedAdmin()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("UserRole") == "Admin";
        }

        public bool IsAuthenticatedClient()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("UserRole") == "Client";
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("Username");
        }

        public int GetUserId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
        }


        public string GetCurrentRole()
        {
            if (IsAuthenticatedAdmin())
            {
                return "Admin";
            }
            else if (IsAuthenticatedClient())
            {
                return "Client";
            }
            else
            {
                return "";
            }
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("UserRole"));
        }
    }

}
