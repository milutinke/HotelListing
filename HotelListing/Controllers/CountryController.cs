using AutoMapper;
using HotelListing.DTO;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<CountryController>? _logger;
        private readonly IMapper? _mapper;

        public CountryController(IUnitOfWork? unitOfWork, ILogger<CountryController>? logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork!.Countries.GetAllAsync();
                var results = _mapper!.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger!.LogError(e, $"Something went wrong when fetching countries in {nameof(GetCountries)}!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork!.Countries.GetAsync(q => q.Id == id, new List<string> { "Hotels" });
                return Ok(_mapper!.Map<CountryDTO>(country));
            }
            catch (Exception e)
            {
                _logger!.LogError(e, $"Something went wrong when fetching countries in {nameof(GetCountries)}!");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
