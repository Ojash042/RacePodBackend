using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Model;
using RacePodBackend.Services;
using System.Web;
using RacePodBackend.Errors;

namespace RacePodBackend.Controllers;

[Route("api/getNextPage")]
[ApiController]
public class GetNextRangeController : ControllerBase{
    private readonly ILogger<GetNextRangeController> _logger;
    private readonly IDataServices _dataServices;
    public GetNextRangeController(ILogger<GetNextRangeController> logger, IDataServices dataServices){
        _logger = logger;
        _dataServices = dataServices;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get([FromQuery]Guid guid, [FromQuery]int page){
        try{
            return Ok(_dataServices.GetRangeOfEpisodes(guid, page));
        }
        catch (ItemListFulfilledException e){
            return BadRequest(e.Message);
        }
    }
    
}