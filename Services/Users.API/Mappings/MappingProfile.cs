using AutoMapper;
using BuildingBlocks.Extensions;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;
using Users.API.Extensions;

namespace Users.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, MemberDto>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o =>
                o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<MemberUpdateDto, User>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));

    }
}