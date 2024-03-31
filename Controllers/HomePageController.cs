using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Model;
using RacePodBackend.Services;

namespace RacePodBackend.Controllers;

[ApiController]
[Route("/")]
public class HomePageController : ControllerBase{
    private readonly ILogger<HomePageController> _logger;
    private readonly IDataServices _dataServices;
    public HomePageController(ILogger<HomePageController> logger, IDataServices dataServices){
        _logger = logger;
        _dataServices = dataServices;
    }

    [HttpGet(Name = "HomePage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public List<PodcastSeries> Get(){
        return _dataServices.GetAllSeries();
    }
}
