using RacePodBackend.ViewModels;
using RacePodBackend.Model;

namespace RacePodBackend.Services;

interface IDataServices{
    public PodcastSeries InitialResults(Guid id);
    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start,int range);
}
