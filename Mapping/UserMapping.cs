using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping() {
            CreateMap<Person, UserResponse>();
            CreateMap<Person, CurrentUserResponse>();
            CreateMap<UserRegisterRequest, Person>();
        }
    }
}
