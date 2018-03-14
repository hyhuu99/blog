using AutoMapper;
using blog.Models;
using DTO;
using Utility.AnotherFunction;

namespace blog.AutoMapper
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WebMappingProfile>();
            });
            return config;
        }
    }
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<AccountVM.RegisterViewModel, UserDTO>().ReverseMap();
            CreateMap<UserDTO, AccountVM.RegisterViewModel>().ReverseMap();
            CreateMap<AccountVM.LoginViewModel, UserDTO>().ReverseMap();
            CreateMap<UserDTO, AccountVM.LoginViewModel>().ReverseMap();
            CreateMap<PostVM.PostCreateVM, PostDTO>()
                .ForMember(d => d.ID, opt => opt.MapFrom(s => s.PostId))
                .ReverseMap();
            CreateMap<PostDTO, PostVM.PostCreateVM>()
                .ForMember(d => d.PostId, opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.RowVersion,opt => opt.MapFrom(s => s.RowVersion))
                .ReverseMap();
            CreateMap<CommentVM, CommentDTO>().ReverseMap();

            CreateMap<CommentDTO, CommentVM>()
                .ForMember(d => d.ImageUser, opt => opt.MapFrom(s => s.ImageUser))
                .ReverseMap();

            CreateMap<PostDTO, PostVM.PostDetailVM>()
                .ForMember(d => d.PostId,opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => ImageFunction.ConvertImage(s.Image)))
                .ForMember(d => d.ListComment, opt => opt.MapFrom(s =>s.ListCommentDTO))
                .ForMember(d => d.ListTag, opt => opt.MapFrom(s => s.ListTagDTO))
                .ReverseMap();

            CreateMap<PostDTO, PostVM.PostIndexVM>()
                .ForMember(d => d.PostId, opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => ImageFunction.ConvertImage(s.Image)))
                .ForMember(d => d.ImageUser, opt => opt.MapFrom(s => ImageFunction.ConvertImage(s.ImageUser)))
                .ForMember(d => d.ListTag, opt => opt.MapFrom(s => s.ListTagDTO))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ReverseMap();
            CreateMap<TagVM, TagDTO>().ReverseMap();
            CreateMap<TagDTO, TagVM>()
                .ForMember(d => d.TagId, opt => opt.MapFrom(s => s.TagId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.NameTag))
                .ReverseMap();
            //CreateMap<PostVM.PostCreateVM, PostDTO>().ReverseMap();
            //CreateMap<PostDTO, PostVM.PostCreateVM>().ReverseMap();

        }
    }

}
