using Microsoft.AspNetCore.Mvc;

namespace RacePodBackend.Controllers;

[Route("/browse")]
public class PodBrowser : ControllerBase {
	// GET
	[HttpGet("/{guid}")]
	public IActionResult Get(String guid) {
		return Ok(guid);
	}
}