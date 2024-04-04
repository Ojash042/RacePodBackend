using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Services;

namespace RacePodBackend.Controllers;

[ApiController]

public class UpdateEpisodesController: ControllerBase{
    
    private readonly ILogger<UpdateEpisodesController> _logger;
    private readonly FeedReader _reader;
    public UpdateEpisodesController(ILogger<UpdateEpisodesController> logger, FeedReader reader){
        _logger = logger;
        _reader = reader;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("/UpdateEpisodes/{guid}")]
    public IActionResult Get(Guid guid){
        _reader.updateEpisodeRecords(guid);
        return Ok();
    }
}