using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class AttendanceMapper : Profile
    {
        public AttendanceMapper() {
            CreateMap<AttendanceRequest, Attendance>();
            CreateMap<Attendance, AttendanceResponse>();

        }
    }
}
