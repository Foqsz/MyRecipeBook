using MyRecipeBook.Domain.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{  
    private readonly IUserDeleteOnlyRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteUserAccountUseCase( 
        IUserDeleteOnlyRepository userRepository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    { 
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(Guid userIdentifier)
    {
        await _blobStorageService.DeleteContainer(userIdentifier);

        await _userRepository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}
