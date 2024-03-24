using RacePodBackend.ViewModels;
using RacePodBackend.Model;
using RacePodBackend.Data;

namespace RacePodBackend.Services;

public class DataServices : IDataServices{
    private readonly ILogger<DataServices> _logger;
    private readonly ApplicationDbContext _dbContext;

    DataServices(ILogger<DataServices> logger, ApplicationDbContext dbContext){
        _logger = logger;
        _dbContext = dbContext;
    }

    public PodcastSeries InitialResults(Guid id) {
        _dbContext.PodcastSeries
            .Where(podcastSeries => podcastSeries.PodcastSeriesId == id);
        return new PodcastSeries();
    }

    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start, int range=30){
        return new List<PodcastEpisode>();
    }

    public List<PodcastSeries> GetAllSeries(){
        var seriesList =  _dbContext.PodcastSeries.ToList();
        return seriesList;
    }
}
