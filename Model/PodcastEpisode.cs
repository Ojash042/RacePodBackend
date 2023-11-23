namespace RacePodBackend.Model; 

public class PodcastEpisode {
	public string Title { get; set; }
	public DateTime PublishedDate { get; set; }
	public string ImageUrl { get; set; }
	public string EpisodeDescription {get; set; }
	public string AudioSource { get; set; }
	public string AudioType { get; set; }
}