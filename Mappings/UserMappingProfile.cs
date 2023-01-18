using AutoMapper;
using Core.Dto;
using Core.Entities;

namespace Mappings
{
    internal class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}
