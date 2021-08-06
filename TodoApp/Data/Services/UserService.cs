using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace TodoApp.Data.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILogger _logger;

        public UserService(AuthenticationStateProvider authenticationStateProvider, ILogger<UserService> logger)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
        }

        public async Task<string> GetUserIdAsync()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return GetUserId(authenticationState);
        }

        public string GetUserId(AuthenticationState authenticationState)
        {
            GuardAgainstUnauthenticated(authenticationState);
            return authenticationState.User
                .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                .Value;
        }


        private void GuardAgainstUnauthenticated(AuthenticationState authenticationState)
        {
            bool error = false;
            if (authenticationState == null)
            {
                _logger.LogError("authenticationState is null");
                error = true;
            }
            if (authenticationState.User == null)
            {
                _logger.LogError("authenticationState.User is null");
                error = true;
            }
            if (authenticationState.User.Identity == null)
            {
                try
                {
                    _logger.LogError(
                        "authenticationState.User.Identity is null. Claims - {claims}",
                        string.Join(";", authenticationState.User.Claims.Select(c => $"{c.Type}: {c.Value}")));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                error = true;
            }

            error |= !authenticationState.User.Identity.IsAuthenticated;

            if (error)
            {
                throw new InvalidOperationException("User is not logged in.");
            }
        }
    }
}
