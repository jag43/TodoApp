using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TodoApp.Data.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetUserId()
        {
            GuardAgainstUnauthenticated();

            return _httpContextAccessor.HttpContext.User
                .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                .Value;
        }

        private void GuardAgainstUnauthenticated()
        {
            bool error = false;
            if(_httpContextAccessor.HttpContext.User == null)
            {
                _logger.LogError("HttpContext.User is null");
                error = true;
            }
            if (_httpContextAccessor.HttpContext.User.Identity == null)
            {
                try
                {
                    _logger.LogError(
                        "HttpContext.User.Identity is null. Claims - {claims}",
                        string.Join(";", _httpContextAccessor.HttpContext.User.Claims.Select(c => $"{c.Type}: {c.Value}")));
                }catch(Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                error = true;
            }

            error |= !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (error)
            {
                throw new InvalidOperationException("User is not logged in.");
            }
        }
    }
}
