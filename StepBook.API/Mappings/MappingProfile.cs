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
        CreateMap<Message, MessageDto>()
            .ForMember(x => x.SenderPhotoUrl, opt
                => opt.MapFrom(src
                    => src.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(x => x.RecipientPhotoUrl, opt
                => opt.MapFrom(src
                    => src.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
    }
}