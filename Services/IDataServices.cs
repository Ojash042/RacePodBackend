using RacePodBackend.ViewModels;
using RacePodBackend.Model;

namespace RacePodBackend.Services;

public interface IDataServices{
    public List<PodcastSeries> GetAllSeries();
    public PodcastSeries InitialResults(Guid id);
    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start,int range);
}
