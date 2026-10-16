using GaiaPressBlazorWebAPP.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GaiaPressBlazorWebAPP.Components.Account
{
    internal sealed class IdentityUserAccessor
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityRedirectManager _redirectManager;

        public IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
        {
            _userManager = userManager;
            _redirectManager = redirectManager;
        }

        public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user is null)
            {
                _redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{_userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
