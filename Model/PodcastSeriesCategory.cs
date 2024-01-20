using Microsoft.EntityFrameworkCore;

namespace RacePodBackend.Model;

[Keyless]
public class PodcastSeriesCategory {
	public PodcastSeries PodcastSeries { get; set; } = null!;
	public Guid PodcastSeriesId { get; set; }
	public Category Category { get; set; } = null!;
	public Guid CategoryId { get; set; }
}