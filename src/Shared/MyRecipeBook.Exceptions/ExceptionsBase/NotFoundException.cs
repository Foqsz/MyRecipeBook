namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class NotFoundException : MyRecipeBookException
{
    public NotFoundException(string message) : base(message) // recebo a mensagem de erro que será exibida ao usuário
    {
        
    }
}
