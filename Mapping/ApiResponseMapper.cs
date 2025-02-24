using AutoMapper;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Mapping
{
    public class ApiResponseMapper : Profile
    {
        public ApiResponseMapper()
        {
            CreateMap(typeof(ApiResponse<>), typeof(ApiResponse<>));
        }
    }
}
