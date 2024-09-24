using Account.API.Features.Account;
using AutoMapper;
using StepBook.Domain.Entities;

namespace Account.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
    }
}