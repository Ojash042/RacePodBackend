using RacePodBackend.ViewModels;
using RacePodBackend.Model;

namespace RacePodBackend.Services;

interface IDataServices{
    public SeriesEpisodeCategory InitialResults(Guid id);
    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start,int range);
}
