using AutoMapper;
using Database;
using Database.Code;
using DTO;
using System;

namespace Services.AutoMapper
{
    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, User>()
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.DisplayName))
                .ForMember(d => d.Role,opt => opt.MapFrom(s => 1))
                .ForMember(d => d.CreateDate,opt => opt.MapFrom(s => DateTime.Now))
                .ReverseMap();
            CreateMap<Post, PostDTO>()
                .ForMember(d => d.NameTag,opt => opt.Ignore())
                .ForMember(d => d.UserId,opt => opt.Ignore())
                .ForMember(d => d.AuthorName,opt => opt.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.ImageUser,opt => opt.Ignore())
                .ForMember(d => d.Email, opt => opt.Ignore())
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(s => s.CreateDate.Value.ToString("dd-MM-yyyy")))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image))
                .ForMember(d => d.ListTagDTO, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<PostDTO, Post>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)StatusCode.ACTIVE))
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(s => DateTime.Now))
                .ForMember(d => d.Image,opt => opt.MapFrom(s => s.Image))
                .ForMember(d => d.Slug, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.User,opt => opt.Ignore())
                .ReverseMap();



            CreateMap<CommentDTO, Comment>().ReverseMap();
            CreateMap<Tag, TagDTO>()
                .ForMember(d => d.NameTag, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.SlugTag, opt => opt.MapFrom(s => s.Slug))
                .ForMember(d => d.TagId, opt => opt.MapFrom(s => s.ID))
                .ReverseMap();
        }
    }
}
