using AutoMapper;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Mapping
{
    public class AddressMapper : Profile
    {
        public AddressMapper() {
            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressResponse>();
            CreateMap<Provinces, ProvinceResponse>();
            CreateMap<Districts, DistrictResponse>();
            CreateMap<Wards, WardResponse>();
        }  
    }
}
