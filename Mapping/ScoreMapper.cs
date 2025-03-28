using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class ScoreMapper : Profile
    {
        public ScoreMapper() { 
            CreateMap<Student_Score, ScoreOfSubjectResponse>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Student_Score, ScoreBoardResponse>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateScoreOfSubjectRequest, Student_Score>()
                .ForAllMembers(opts => opts.Condition((src, dest,srcMember) => srcMember != null));
        }
    }
}
