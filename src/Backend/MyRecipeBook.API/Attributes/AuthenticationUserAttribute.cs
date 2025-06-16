using Microsoft.AspNetCore.Mvc;

namespace MyRecipeBook.API.Attributes;

public class AuthenticationUserAttribute : TypeFilterAttribute
{
    public AuthenticationUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
