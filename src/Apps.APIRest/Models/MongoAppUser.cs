using Apps.Domain.Business.Interfaces;
using MongoDB.Bson;
using System.Security.Claims;

namespace Apps.APIRest.Models
{
    public class MongoAppUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public MongoAppUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public ObjectId GetUserId()
        {
            return IsAuthenticated() ? ObjectId.Parse(_accessor.HttpContext.User.GetUserId()) : default;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
