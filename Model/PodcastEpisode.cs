using System.ComponentModel.DataAnnotations;

namespace RacePodBackend.Model; 

public class PodcastEpisode {
	public Guid PodcastEpisodeId { get; set; }
	[StringLength(128)]
	public string? Title { get; set; }
	public DateTime PublishedDate { get; set; }
	
	[StringLength(512)]
	public string? ImageUrl { get; set; }
	
	public string? EpisodeDescription {get; set; }
	[StringLength(512)]
	public string? AudioSource { get; set; }
	[StringLength(32)]
	public string? AudioType { get; set; }
	public PodcastSeries PodcastSeries = null!;
	public Guid PodcastSeriesId;
	public UserData? UserData { get; set; }
}