namespace MyRecipeBook.Domain.Repositories.UnitOfWork;
public interface IUnitOfWork
{
    public Task Commit();
}
