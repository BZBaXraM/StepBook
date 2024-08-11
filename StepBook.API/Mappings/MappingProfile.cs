namespace StepBook.API.Mappings;

/// <inheritdoc />
public class MappingProfile : Profile
{
    /// <inheritdoc />
    public MappingProfile()
    {
        CreateMap<User, MemberDto>()
            .ForMember(x =>
                x.PhotoUrl, opt => opt
                .MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(x =>
                x.Age, opt => opt
                .MapFrom(src => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.SenderUsername,
                opt =>
                    opt.MapFrom(src => src.Sender.UserName))
            .ForMember(dest => dest.RecipientPhotoUrl,
                opt =>
                    opt.MapFrom(src =>
                        src.Recipient.Photos.FirstOrDefault(p
                            => p.IsMain)!.Url));
        CreateMap<DateTime, DateTime>().ConvertUsing(d =>
            DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc)
            : null);
    }
}