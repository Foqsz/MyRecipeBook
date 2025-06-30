using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class ExistActiveUserWithIdentifierException : MyRecipeBookException
{
    public ExistActiveUserWithIdentifierException(string message) : base(message)
    {
    }
    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
