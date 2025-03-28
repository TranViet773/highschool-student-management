using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Controllers;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<Person> _userManager;

        public AddressService(ApplicationDbContext context, IMapper mapper, ILogger<AddressService> logger, UserManager<Person> userManager)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<ApiResponse<AddressResponse>> CreateAddressAsync(AddressRequest addressRequest)
        {
            var user = await _userManager.FindByIdAsync(addressRequest.idUser.ToString());
            if(user is null)
            {
                _logger.LogError("User is not found");
                return new ApiResponse<AddressResponse>
                {
                    Code = "404",
                    Message = "User is not found!",
                    Data = null
                };
            }

            var address = _mapper.Map<AddressRequest, Address>(addressRequest);
            address.Person_Id = addressRequest.idUser.ToString();
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return new ApiResponse<AddressResponse>
            {
                Code = "200",
                Message = "Create new Address is successfully!",
                Data = _mapper.Map<AddressResponse>(address)
            };
        }

        public async Task<ApiResponse<AddressResponse>> GetAddressAsync(Guid idUser)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.Person_Id == idUser.ToString());
            if (address is null)
            {
                return new ApiResponse<AddressResponse>
                {
                    Code = "400",
                    Message = "Get address failed!",
                    Data = null,
                };
            }
            var addressResponse = _mapper.Map<AddressResponse>(address);
            var ward = _context.Wards.FirstOrDefault(w => w.Ward_Id == address.Ward_Id);
            var district = _context.Districts.FirstOrDefault(d => d.Districts_Id == ward.Districts_Id);
            var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Province_Id == district.Province_Id);
            addressResponse.Province_Name = province.Province_Name;
            addressResponse.Ward_Name = ward.Ward_Name;
            addressResponse.District_Name = district.Districts_Name;
            return new ApiResponse<AddressResponse>
            {
                Code = "200",
                Message = "Get address successfully!",
                Data= addressResponse,
            };
        }

        public async Task<ApiResponse<List<DistrictResponse>>> GetAllDistrictByProvinceAsync(int provinceId)
        {
            var districts = await _context.Districts.Where(w => w.Province_Id == provinceId).ToListAsync();
            return new ApiResponse<List<DistrictResponse>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<List<DistrictResponse>>(districts)
            };
        }

        public async Task<ApiResponse<List<ProvinceResponse>>> GetAllProvinceAsync()
        {
            var provinces = await _context.Provinces.ToListAsync();
            return new ApiResponse<List<ProvinceResponse>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<List<ProvinceResponse>>(provinces)
            };
        }

        public async Task<ApiResponse<List<Wards>>> GetAllWardByDistrictIdAsync(int districtId)
        {
            var wards = await _context.Wards.Where(w => w.Districts_Id == districtId).ToListAsync();
            return new ApiResponse<List<Wards>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<List<Wards>>(wards)
            };
        }


        public async Task<ApiResponse<AddressResponse>> UpdateAddressForUserAsync(AddressRequest request)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Person_Id == request.idUser);
            if (address == null) {
                _logger.LogError("Address is not found!");
                return new ApiResponse<AddressResponse>
                {
                    Code = "404",
                    Message = "Address is not found!",
                    Data = null
                };
            }
            address.Address_Detail = request.Address_Detail;
            address.Ward_Id = request.Ward_Id;
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return new ApiResponse<AddressResponse>
            {
                Code = "200",
                Message = "Update address is successfully!",
                Data = _mapper.Map<AddressResponse>(address)    
            };

        }
    }
}
