namespace RacePodBackend.Model; 

public class PodcastSeries {
	
	public string Author { get; set; }
	public string Title { get; set; }
	public DateTime PublishedDate { get; set; }
	public string Url { get; set; }
	public string language { get; set; }
	public string copyright { get; set; }
	public string? Keywords { get; set; }
	public List<string> category { get; set; } = new List<string>();
	public string Description { get; set; }
	public string Image { get; set; }
	public List<PodcastEpisode> Episodes { get; set; }
}