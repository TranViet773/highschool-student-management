using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class ClassMapper : Profile
    {
        public ClassMapper() {
            CreateMap<ClassRequest, Classes>();
            CreateMap<Classes, ClassResponse>();
            CreateMap<ClassRequest, Teacher_Class>();
        }
    }
}
