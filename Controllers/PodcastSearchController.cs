using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Services;

namespace RacePodBackend.Controllers;

[ApiController]
[Route("/podcast")]
public class PodcastSearchController{
    private readonly ILogger<PodcastSearchController> _logger;
    private readonly DataServices _dataServices;

    public PodcastSearchController(ILogger<PodcastSearchController> logger, DataServices dataServices){
        _logger = logger;
        _dataServices = dataServices;
    }
}