using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Attributes;

public class AuthenticationUserAttribute : TypeFilterAttribute
{
    public AuthenticationUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
