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
            CreateMap<Teacher_Class, ClassResponse>()
                    .ForMember(dest => dest.Classes_Name, opt => opt.MapFrom(src => src.Class.Classes_Name))
                    .ForMember(dest => dest.Classes_Id, opt => opt.MapFrom(src => src.Class.Classes_Id))
                    .ForMember(dest => dest.Classes_Quantity, opt => opt.MapFrom(src => src.Class.Classes_Quantity))
                    .ForMember(dest => dest.Classes_Code, opt => opt.MapFrom(src => src.Class.Classes_Code));
        }
    }
}
