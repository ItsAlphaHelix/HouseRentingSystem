namespace HouseRentingSystem.Web.Extensions
{
    using System.Security.Claims;
    public static class ClaimPrincipleExtension
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
