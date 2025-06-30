using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message]; //mesma coisa que fazer new List<string> { Message };

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
