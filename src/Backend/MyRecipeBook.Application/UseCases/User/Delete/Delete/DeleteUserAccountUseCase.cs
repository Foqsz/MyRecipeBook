
using AutoMapper;
using MyRecipeBook.Domain.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IUserUpdateOnlyRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser, 
        IMapper mapper, 
        IUserUpdateOnlyRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Execute(Guid userIdentifier)
    {
        var loggedUser = _loggedUser.User();


    }
}
