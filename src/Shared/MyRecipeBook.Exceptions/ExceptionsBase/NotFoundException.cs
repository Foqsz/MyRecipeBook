using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class NotFoundException : MyRecipeBookException
{
    public NotFoundException(string message) : base(message) // recebo a mensagem de erro que será exibida ao usuário
    {
        
    }

    public override IList<string> GetErrorMessages() => [Message]; // mesma coisa que fazer new List<string> { Message };

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
