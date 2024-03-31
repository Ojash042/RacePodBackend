using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using RacePodBackend.Model;
using RacePodBackend.Data;
using RacePodBackend.ViewModels;

namespace RacePodBackend.Services;

public class DataServices : IDataServices{
    private readonly ILogger<DataServices> _logger;
    private readonly ApplicationDbContext _dbContext;

    public DataServices(ILogger<DataServices> logger, ApplicationDbContext dbContext){
        _logger = logger;
        _dbContext = dbContext;
    }

    public PodcastSeries InitialResults(Guid id){
        PodcastSeries seriesItem = _dbContext.PodcastSeries
            .Include(p => p.Episodes.OrderByDescending(e => e.PublishedDate).Take(30))
            .Include(p => p.Category)
            .First(series => series.PodcastSeriesId == id);
            seriesItem.Category = seriesItem.Category.Select(category => new Category{ CategoryName = category.CategoryName })
                .ToList();
            return seriesItem;
    }

    public List<PodcastEpisode> GetRangeOfEpisodes(Guid id, int start, int range=30){
        return new List<PodcastEpisode>();
    }

    public List<PodcastSeries> GetAllSeries(){
        var seriesList =  _dbContext.PodcastSeries.ToList();
        return seriesList;
    }

}
