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

            CreateMap<UserUpdateRequest, Person>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserRegisterRequest, Teacher>().IncludeBase<UserRegisterRequest, Person>();
            CreateMap<UserRegisterRequest, Students>()
            .IncludeBase<UserRegisterRequest, Person>();
            CreateMap<UserRegisterRequest, Parents>()
            .IncludeBase<UserRegisterRequest, Person>();
            CreateMap<UserRegisterRequest, SystemAdmin>()
            .IncludeBase<UserRegisterRequest, Person>();
            CreateMap<UserRegisterRequest, ManagementStaff>()
            .IncludeBase<UserRegisterRequest, Person>();
        }
    }
}
