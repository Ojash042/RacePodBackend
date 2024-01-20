using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RacePodBackend.Errors;
using RacePodBackend.Model;
using RacePodBackend.Services;

namespace RacePodBackend.Controllers; 


[ApiController]
[Route("/add/")]
public class PodAdditionController(ILogger<PodAdditionController> logger, FeedReader feedReader)
	: ControllerBase {
	private readonly ILogger<PodAdditionController> _loggger = logger;
	private readonly FeedReader _feedReader = feedReader;
	
	[HttpPost(Name = "AddPodcast")]
	[ProducesResponseType<PodcastSeries>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult PostPodAddition(String url) {
		PodcastSeries val;
		try {
			if (!(url.StartsWith("https://") || url.StartsWith("http://"))) {
				url = $"https://{url}";
			}
			//val = _feedReader.AddNewPodcastSeries(url);
			_feedReader.AddPodcast(url);
			val = new PodcastSeries();
		}
		catch (InvalidRssException e) {
			return BadRequest(e.Message);
		}
		catch (XmlException e) {
			return BadRequest("The url you have provided is not a valid podcast url");
		}
		return Ok(val);
	}
}