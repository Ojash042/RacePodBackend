namespace RacePodBackend.Model;

public class UserData {
	public Guid UserDataId { get; set; }
	public Guid PodcastEpisodeId { get; set; }
	public PodcastEpisode PodcastEpisode { get; set; } = null!;
	public Int32 Duration { get; set; }
}