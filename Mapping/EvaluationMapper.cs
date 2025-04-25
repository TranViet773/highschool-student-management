using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class EvaluationMapper : Profile
    {
        public EvaluationMapper() {
            CreateMap<EvaluationRequest, Evaluation>();
            CreateMap<Evaluation, EvaluationResponse>();
        }
    }
}
