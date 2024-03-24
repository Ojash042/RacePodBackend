using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using RacePodBackend.Data;
using System.Collections.Generic;
using System.Diagnostics;
using RacePodBackend.Model;

namespace RacePodBackend.Services; 

public class FeedReader {
	//private string[] _rssList = new string[]{"https://feeds.libsyn.com/65267/rss","https://feeds.megaphone.fm/darknetdiaries"};
	private readonly ILogger<FeedReader> _logger;
	private readonly ApplicationDbContext _applicationDbContext;

	private XNamespace _nsItunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
	private XNamespace _nsContent = "http://purl.org/rss/1.0/modules/content/";
	
	public FeedReader(ILogger<FeedReader> logger, ApplicationDbContext applicationDbContext) {
		_applicationDbContext = applicationDbContext;
		_logger = logger;
	}

	public void updateEpisodeRecords(Guid podcastSeriesID){
		List<PodcastEpisode> episodesInDb = _applicationDbContext.PodcastEpisodes.Where(e => e.PodcastSeriesId == podcastSeriesID).ToList();
		PodcastSeries podcastSeries = _applicationDbContext.PodcastSeries.First(series=>series.PodcastSeriesId == podcastSeriesID);
		using (var trans = _applicationDbContext.Database.BeginTransaction())
		using (var reader = XmlReader.Create(podcastSeries.RssUrl)){
			SyndicationFeed feed = SyndicationFeed.Load(reader);
			var enumerator = feed.Items.GetEnumerator();

			while(podcastSeries.PublishedDate < enumerator.Current?.PublishDate.UtcDateTime){
				List<PodcastEpisode> episodes = new();
				PodcastEpisode episode = new();
				var item = enumerator.Current;
				episode.Title = item.Title.Text;
				episode.PublishedDate = item.PublishDate.DateTime;
				episode.EpisodeDescription = item.Summary.Text;
				episode.PodcastSeries = podcastSeries;
				episode.PodcastSeriesId = podcastSeriesID;
				episode.PodcastEpisodeId = Guid.NewGuid();
    				
				var imageNode = item.ElementExtensions.FirstOrDefault(e => e.OuterName.Equals("image")&& e.OuterNamespace == _nsItunes );
				episode.ImageUrl = imageNode?.GetObject<XElement>().Attribute("href")?.Value;
				var enclosure = item.Links.Where(x => x.RelationshipType == "enclosure").FirstOrDefault();
				episode.AudioSource = enclosure?.Uri.OriginalString;
				episode.AudioType = enclosure?.MediaType;
				_applicationDbContext.PodcastEpisodes.Add(episode);
				enumerator.MoveNext();
			}
			podcastSeries.PublishedDate = feed.LastUpdatedTime.UtcDateTime;
			_applicationDbContext.SaveChanges();
			trans.Commit();
			enumerator.Dispose();
		}
	}

	private DateTime parseToDateTime(String dateTime){
		String formattedDate = String.Join(" ",dateTime.Split(" ").ToList().Skip(1).Take(3));
		String formattedTime = dateTime.Split(" ").ToList().ElementAt(4);
		DateOnly parsedDate = DateOnly.Parse(formattedDate);
		TimeOnly parsedTime = TimeOnly.Parse(formattedTime);
		return new DateTime(parsedDate, parsedTime,DateTimeKind.Utc);
	}
	
	public void AddPodcast(String url){
		PodcastSeries podcastSeries = new();
		//url = "https://feeds.libsyn.com/65267/rss";
		using (var trans = _applicationDbContext.Database.BeginTransaction())
		using (var reader = XmlReader.Create(url)) {
			SyndicationFeed feed = SyndicationFeed.Load(reader);

			podcastSeries.Title = feed.Title.Text;
			podcastSeries.PodcastSeriesId = Guid.NewGuid();
			
			
			// Gets each necessary node from the Rss feed then sets the object's value to the corresponding value from the rss feed.
			var itunesAuthor = feed.ElementExtensions
				.FirstOrDefault(e=> e.OuterName.Equals("author") && e.OuterNamespace == _nsItunes);
			podcastSeries.Author = itunesAuthor.GetObject<XElement>().Value;
			
			var keywords = feed.ElementExtensions.FirstOrDefault(e=>e.OuterName.Equals("keywords") && e.OuterNamespace==_nsItunes);
			podcastSeries.Keywords = keywords?.GetObject<XElement>().Value ?? "";
			
			var publishedDate = feed.ElementExtensions.FirstOrDefault(e=> e.OuterName.Equals("pubDate"));
			podcastSeries.PublishedDate = (publishedDate==null) ? DateTime.MinValue : parseToDateTime(publishedDate?.GetObject<XElement>().Value);
			var documentation = feed.Documentation ;
			podcastSeries.Url = (documentation == null)? "" : documentation.Uri.ToString();
			podcastSeries.Language = feed.Language;
			podcastSeries.Copyright = feed.Copyright.Text;

			var val = feed.ElementExtensions.Where(e=>e.OuterName.Equals("category") && e.OuterNamespace == _nsItunes);

			List < Category > categories = _applicationDbContext.Categories.ToList();
			foreach (var i in val){
				String categoryName = i.GetObject<XElement>().Attribute("text").Value;
				Category categoryExists  = categories.FirstOrDefault(cat => cat.CategoryName == categoryName);
				Category category = categoryExists!=null ? categoryExists : new Category()
					{ CategoryName = categoryName, CategoryId = Guid.NewGuid()};
				if (categoryExists == null){
					category.PodcastSeries.Add(podcastSeries);

				}
				podcastSeries.Category.Add(category);
				//_applicationDbContext.Categories.Add(category);
			}
			
			podcastSeries.Description = feed.Description.Text;
			podcastSeries.Image = feed.ImageUrl.OriginalString;
			podcastSeries.RssUrl = url;

			foreach (var item in feed.Items){
				List<PodcastEpisode> episodes = new();
				PodcastEpisode episode = new();

				episode.Title = item.Title.Text;
				// _logger.LogInformation($"Episode {episode.Title}");
				episode.PublishedDate = item.PublishDate.UtcDateTime;
				var description = item.ElementExtensions.FirstOrDefault(e => e.OuterName=="description");
				var content = item.ElementExtensions.FirstOrDefault(e => e.OuterName == "encoded" && e.OuterNamespace==_nsContent);
				var objects = (description==null) ? content.GetObject<XElement>().Value : description.GetObject<XElement>().Value ;
				// _logger.LogError($"Summary Text State = {item.Summary == null}");
				episode.EpisodeDescription =  (item.Summary==null) ? objects : item.Summary.Text;
				episode.PodcastEpisodeId = Guid.NewGuid();

				var imageNode =
					item.ElementExtensions.FirstOrDefault(e =>
						e.OuterName.Equals("image") && e.OuterNamespace == _nsItunes);
				episode.ImageUrl = imageNode.GetObject<XElement>().Attribute("href").Value;
				var enclosure = item.Links.Where(x => x.RelationshipType == "enclosure").FirstOrDefault();
				episode.AudioSource = enclosure.Uri.OriginalString;
				episode.AudioType = enclosure.MediaType;
				episode.PodcastSeriesId = podcastSeries.PodcastSeriesId;
				episode.PodcastSeries = podcastSeries;
				podcastSeries.Episodes.Add(episode);
				_applicationDbContext.PodcastEpisodes.Add(episode);
			}
			
			_applicationDbContext.PodcastSeries.Add(podcastSeries);
			_applicationDbContext.SaveChangesAsync();
			trans.Commit();
		}
	}
	public PodcastSeries AddNewPodcastSeries(String url) {
		//_logger.LogInformation("Starting: Add new Podcast Series");
		var watch = System.Diagnostics.Stopwatch.StartNew();
		PodcastSeries podcastSeries = new PodcastSeries();
		var xml = XDocument.Load(url);
		var channel = xml.Descendants("channel");
		podcastSeries.Author = xml.Descendants(_nsItunes + "author").FirstOrDefault()?.Value ?? "";
		Console.WriteLine(podcastSeries.Author);
		podcastSeries.Keywords = xml.Descendants(_nsItunes + "keywords").FirstOrDefault()?.Value ?? "";
		podcastSeries.Title = channel.Elements("title").FirstOrDefault()?.Value ?? "";
		//podcastSeries.PublishedDate = DateTime.ParseExact(channel.Elements("pubDate").FirstOrDefault().Value);
		podcastSeries.Url = channel.Elements("link").FirstOrDefault()?.Value ?? "";
		podcastSeries.Language = channel.Elements("language").FirstOrDefault().Value ?? "";
		podcastSeries.PodcastSeriesId = Guid.NewGuid();
		if (channel.Elements("copyright").First() is XCData) {
			podcastSeries.Copyright = ((XCData)(channel.Elements("copyright").First().FirstNode)).Value;
		}
		else {
			podcastSeries.Copyright = channel.Elements("copyright").FirstOrDefault().Value ?? "";
		}

		List<Category> categories = _applicationDbContext.Categories.ToList();

		foreach (var categoryElement in channel.Elements(_nsItunes + "category")) {
			string categoryName = categoryElement.Attribute("text").Value;
			var categoryExists = categories.FirstOrDefault(category => category.CategoryName == categoryName);
			Category category = categoryExists != null ? categoryExists : new Category() { CategoryId = Guid.NewGuid(), CategoryName = categoryName };
			//category.PodcastSeries.Add(podcastSeries);
			_logger.LogError($"Category Exits = {(categoryExists != null)} for {category.CategoryName}");
			if (categoryExists == null){
				_logger.LogError($"Category = {category.CategoryId}, {category.CategoryName}");
				podcastSeries.Category.Add(category);
				//_applicationDbContext.Categories.Add(category);
			}
			/*if (categoryExists == null) {
				_applicationDbContext.Categories.Add(category);
			}*/
		};

		if (xml.Descendants("desciption").FirstOrDefault() is XCData) {
			podcastSeries.Description = ((XCData)(xml.Element("description").FirstNode)).Value;
		}
		else {
			podcastSeries.Description = xml.Descendants("description").FirstOrDefault().Value ?? "";
		}
		podcastSeries.Image = xml.Descendants(_nsItunes + "image").FirstOrDefault().Attribute("href").Value;

		List<PodcastEpisode> episodeList = new List<PodcastEpisode>();
		foreach (var item in xml.Descendants("item")) {
			PodcastEpisode episode = new PodcastEpisode();
			episode.Title = item.Element("title").Value;
			episode.PublishedDate = DateTime.Parse(item.Element("pubDate").Value);
			episode.ImageUrl = item.Element(_nsItunes + "image").Value;

			episode.EpisodeDescription = item.Element(_nsContent + "encoded").Value;

			episode.AudioType = item.Element("enclosure").Attribute("type").Value;
			episode.AudioSource = item.Element("enclosure").Attribute("url").Value;
			//_logger.LogCritical($"{item.Element("guid").Value}");
			try {
				episode.PodcastEpisodeId = (Guid.Parse(item.Element("guid").Value));
			}
			catch (Exception e) when (e is XmlException || e is FormatException) {
				episode.PodcastEpisodeId = Guid.NewGuid();
			}
			
			episode.PodcastSeries = podcastSeries;
			episode.PodcastSeriesId = podcastSeries.PodcastSeriesId;
			episodeList.Add(episode);
			_applicationDbContext.PodcastEpisodes.Add(episode);
			
		}
		podcastSeries.Episodes = episodeList;
		_applicationDbContext.PodcastSeries.Add(podcastSeries);
		_applicationDbContext.SaveChangesAsync();
		watch.Stop();
		Console.WriteLine($"{watch.Elapsed.Seconds} seconds");
		return podcastSeries;
	}
}

