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
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(statusCode:StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Produces("application/json")]
	public IActionResult Post([FromForm] String url) {
		PodcastSeries val;
		//String url = Request.Form["url"]!;
		try {
			if (!(url.StartsWith("https://") || url.StartsWith("http://"))) {
				url = $"https://{url}";
			}
			//val = _feedReader.AddNewPodcastSeries(url);
			//val =_feedReader.AddPodcast(url);
			_feedReader.AddPodcast(url);
		}
		catch (InvalidRssException e) {
			return BadRequest(e.Message);
		}
		catch (XmlException e) {
			return BadRequest("The url you have provided is not a valid podcast url");
		}
		return Ok();
	}
}