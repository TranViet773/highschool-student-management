using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IAddressService 
    {
        Task<ApiResponse<AddressResponse>> GetAddressAsync(Guid idUser);
        Task<ApiResponse<AddressResponse>> CreateAddressAsync(AddressRequest addressRequest);
        Task<ApiResponse<AddressResponse>> UpdateAddressForUserAsync(AddressRequest address);
        Task<ApiResponse<List<ProvinceResponse>>> GetAllProvinceAsync();
        Task<ApiResponse<List<DistrictResponse>>> GetAllDistrictByProvinceAsync(int provinceId);
        Task<ApiResponse<List<Wards>>> GetAllWardByDistrictIdAsync(int districtId);   
    }
}
