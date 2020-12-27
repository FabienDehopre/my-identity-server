namespace MyIdentityServer4.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using IdentityServer4.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using MyIdentityServer4.ViewModels;

    public static class Extensions
    {
        /// <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <returns></returns>
        public static bool IsNativeClient(this AuthorizationRequest context) =>
            !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) &&
            !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);

        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }

        public static string GetEmail(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Email);

        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelState)
        {
            foreach (var pair in modelState)
            {
                for (var i = 0; i < pair.Value.Errors.Count; i++)
                {
                    yield return pair.Value.Errors[i].ErrorMessage;
                }
            }
        }
    }
}
