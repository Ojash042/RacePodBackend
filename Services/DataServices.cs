using RacePodBackend.ViewModels;
using RacePodBackend.Model;
using RacePodBackend.Data;

namespace RacePodBackend.Services;

class DataServices : IDataServices{
    private readonly ILogger<DataServices> _logger;
    private readonly ApplicationDbContext _dbContext;

    DataServices(ILogger<DataServices> logger, ApplicationDbContext dbContext){
        _logger = logger;
        _dbContext = dbContext;
    }

    public SeriesEpisodeCategory InitialResults(Guid id) {
        _dbContext.PodcastSeries
            .Where(podcastSeries => podcastSeries.PodcastSeriesId == id);
        return new SeriesEpisodeCategory();
    }

    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start, int range=30){
        return new List<PodcastEpisode>();
    }
}
