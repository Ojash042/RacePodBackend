namespace RacePodBackend.Model;

public class Category {
	public Guid CategoryId { get; set; }
	public string CategoryName { get; set; } = null!;
	public List<PodcastSeries> PodcastSeries { get; set; } = new List<PodcastSeries>();

}