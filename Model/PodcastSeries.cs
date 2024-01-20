using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RacePodBackend.Model; 

public class PodcastSeries {
	
	public Guid PodcastSeriesId { get; set; }
	[StringLength(128)]
	public string Author { get; set; } = null!;
	[StringLength(256)]
	public string Title { get; set; } = null!;
	public DateTime PublishedDate { get; init; }
	[StringLength(512)] 
	public string Url { get; set; } = null!;

	[StringLength(32)]
	public string? Language { get; set; }

	[StringLength(64)]
	public string Copyright { get; set; } = null!;

	[StringLength(512)]
	public string? Keywords { get; set; } = null!;
	public List<Category> Category { get; set; } = new List<Category>();
	public string? Description { get; set; }
	[StringLength(512)]
	public string Image { get; set; }

	public List<PodcastEpisode> Episodes { get; set; } = new List<PodcastEpisode>();
}