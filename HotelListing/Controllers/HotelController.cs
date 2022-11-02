using AutoMapper;
using HotelListing.DTO;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<HotelController>? _logger;
        private readonly IMapper? _mapper;

        public HotelController(IUnitOfWork? unitOfWork, ILogger<HotelController>? logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork!.Hotels.GetAllAsync(null, null, new List<string>() { "Country" });
                var results = _mapper!.Map<IList<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger!.LogError(e, $"Something went wrong when fetching countries in {nameof(GetHotels)}!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork!.Hotels.GetAsync(q => q.Id == id, new List<string>() { "Country" });
                return Ok(_mapper!.Map<HotelDTO>(hotel));
            }
            catch (Exception e)
            {
                _logger!.LogError(e, $"Something went wrong when fetching countries in {nameof(GetHotel)}!");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
