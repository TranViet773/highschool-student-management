using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService) { 
            _addressService = addressService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> creatAddressForUser([FromBody]AddressRequest request)
        {
            var response = await _addressService.CreateAddressAsync(request);
            return Ok(response);
        }
        [HttpPost("get-by-user")]
        public async Task<IActionResult> getAddressByUser(Guid idUser)
        {
            var response = await _addressService.GetAddressAsync(idUser);
            return Ok(response);
        }

        [HttpGet("get-all-province")]
        public async Task<IActionResult> getAllProvinceAync()
        {
            var response = await _addressService.GetAllProvinceAsync();
            return Ok(response);
        }

        [HttpGet("get-districts-by-province/{provinceId}")]
        public async Task<IActionResult> getAllDistrictByPronvince(int provinceId)
        {
            var response = await _addressService.GetAllDistrictByProvinceAsync(provinceId);
            return Ok(response);
        }

        [HttpGet("get-wards-by-district/{districtId}")]
        public async Task<IActionResult> getAllWardByDistrict(int districtId)
        {
            var response = await _addressService.GetAllWardByDistrictIdAsync(districtId);
            return Ok(response);
        }

        [HttpPut("update-address-user")]
        public async Task<IActionResult> updateAddressForUser([FromBody]AddressRequest request)
        {
            var response = await _addressService.UpdateAddressForUserAsync(request);
            if (response.Code == "404") return BadRequest(response);
            return Ok(response);
        }
    }
}
