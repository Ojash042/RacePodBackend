using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Model;
using RacePodBackend.Services;

namespace RacePodBackend.Controllers; 


[ApiController]
[Route("/podcast")]
public class PodcastInfoController {
	private readonly ILogger<PodcastInfoController> _loggger;
	private FeedReader _feedReader;
	
	public PodcastInfoController(ILogger<PodcastInfoController> logger, FeedReader feedReader) {
		_feedReader = feedReader;
		_loggger = logger;
	}

	[HttpGet(Name="GetIndex")]
	public PodcastSeries Index(string id) {
		return _feedReader.requestPodcastDetails();
	}
}