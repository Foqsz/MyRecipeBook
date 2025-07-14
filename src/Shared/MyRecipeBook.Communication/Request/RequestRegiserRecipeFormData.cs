using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Communication.Request;
public class RequestRegiserRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}
