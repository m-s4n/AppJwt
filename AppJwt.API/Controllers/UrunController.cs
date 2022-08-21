using AppJwt.Core.Dtos;
using AppJwt.Core.Entities;
using AppJwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppJwt.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly IService<Urun, UrunDto> _urunService;
        private readonly ILogger _logger;

        public UrunController(IService<Urun, UrunDto> urunService, ILoggerFactory loggerFactory)
        {
            _urunService = urunService;
            _logger = loggerFactory.CreateLogger("Urunler");
        }


        [HttpGet]
        public async Task<IActionResult> GetUrunler()
        {

            var result = await _urunService.GetAllAsync();
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }


        [HttpPost]
        public async Task<IActionResult> AddUrun(UrunDto urunDto)
        {
            _logger.LogCritical("Critical Logging");
            var result = await _urunService.AddAsync(urunDto);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUrun(UrunDto urunDto)
        {
            var result = await _urunService.Update(urunDto, urunDto.Id);

            return new ObjectResult(result)
            {
                StatusCode = 200
            };
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrun(int id)
        {
            var result = await _urunService.Remove(id);

            return new ObjectResult(result)
            {
                StatusCode = 200
            };
        }
    }
}
