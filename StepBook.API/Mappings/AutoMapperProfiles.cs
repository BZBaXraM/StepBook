using AutoMapper;
using StepBook.API.DTOs;
using StepBook.API.Models;

namespace StepBook.API.Mappings;

/// <inheritdoc />
public class AutoMapperProfiles : Profile
{
    /// <inheritdoc />
    public AutoMapperProfiles()
    {
        CreateMap<User, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => 
                    src.Photos!.FirstOrDefault(x => x.IsMain)!.Url)); // Use ?. to handle nulls
        CreateMap<Photo, PhotoDto>();
    }
}