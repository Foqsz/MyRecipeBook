using AutoMapper;
using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        RequestDomain(); 
    }

    private void RequestDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
