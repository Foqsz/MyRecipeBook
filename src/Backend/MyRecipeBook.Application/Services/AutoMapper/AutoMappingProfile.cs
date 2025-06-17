using AutoMapper;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        RequestDomain();
        DomainToResponse();
    }

    private void RequestDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>(); 
    }
}
