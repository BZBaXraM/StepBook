namespace StepBook.API.Mappings;

/// <inheritdoc />
public class MappingProfile : Profile
{
    /// <inheritdoc />
    public MappingProfile()
    {
        CreateMap<User, MemberDto>()
            .ForMember(x
                => x.PhotoUrl, opt
                =>
                opt.MapFrom(src
                    => src.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(x => x.Age, opt
                => opt.MapFrom(src
                    => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, User>();
        CreateMap<RegisterDto, User>();
    }
}