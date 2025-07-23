using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class ReevaluationMapper : Profile
    {
        public ReevaluationMapper() {
            CreateMap<ReevaluationRequest, Reevaluation>();
            CreateMap<Reevaluation, ReevaluationResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Students.Fullname))
                .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Students.Email))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Students.Code))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Fullname))
                .ForMember(dest => dest.TeacherEmail, opt => opt.MapFrom(src => src.Teacher.Email))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.Teacher.Code))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subjects.Subject_Name))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Subjects.Subject_Id));
                //.ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Students.ClassStudents));
            //CreateMap<List<ReevaluationResponse>, List<Reevaluation>>();
        }
    }
}
