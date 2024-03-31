using Microsoft.AspNetCore.Mvc;

using RacePodBackend.Services;
using RacePodBackend.Model;

namespace RacePodBackend.Controllers;

[ApiController]
[Route("/browse/[action]")]
public class PodBrowser : ControllerBase {
	private readonly ILogger<PodBrowser> _logger;
	private readonly IDataServices _dataServices;
	
	public PodBrowser(ILogger<PodBrowser> logger, IDataServices dataServices){
		_logger = logger;
		_dataServices = dataServices;
	}
	// GET
	
	[Route("/{guid}")]
	[HttpGet]

	public PodcastSeries Get(Guid guid) {
		var series =  _dataServices.InitialResults(guid);
		return  series;
	}
}
