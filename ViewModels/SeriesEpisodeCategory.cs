using RacePodBackend.Model;

namespace RacePodBackend.ViewModels;

class SeriesEpisodeCategory{
    public PodcastSeries podcastSeries {get; set;} = null!;
    public List<PodcastEpisode> podcastEpisodes {get;set;} = null!;
    public List<Category> categories {get; set;} = null!;
}
