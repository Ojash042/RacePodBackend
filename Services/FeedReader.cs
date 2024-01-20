using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using RacePodBackend.Data;
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

	public bool selector ( SyndicationElementExtension e, string outerName, XNamespace outernameSpace) =>
    				e.OuterName == outerName && e.OuterNamespace == outernameSpace;
	public void AddPodcast(String url) {
		PodcastSeries podcastSeries = new();
		url = "https://feeds.libsyn.com/65267/rss";
		using (var reader = XmlReader.Create(url)) {
			SyndicationFeed feed = SyndicationFeed.Load(reader);

			var _namespaceItunes = new XmlNamespaceManager();
			podcastSeries.Title = feed.Title.Text;
			
			var itunesAuthor = feed.Items.FirstOrDefault().ElementExtensions.
				.FirstOrDefault(e=> selector(e,"author",_nsItunes));
			podcastSeries.Author = itunesAuthor.GetObject<XElement>().Value;
			
			var keywords = feed.Items.FirstOrDefault().ElementExtensions
				.FirstOrDefault(e => e.OuterName=="keywords" && e.OuterNamespace==_nsItunes);
			Console.WriteLine(keywords.GetObject<XElement>().Value);



			/*foreach (var item in feed.Items) {
				var itunesAuthor =
					item.ElementExtensions.FirstOrDefault(e =>
						e.OuterName == "author" && e.OuterNamespace == _nsItunes);

				if (itunesAuthor != null) {
					Console.WriteLine(itunesAuthor.GetObject<XElement>().Value ?? "");
				}

			}*/

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
			Category categoryExists = categories.FirstOrDefault(Category => Category.CategoryName == categoryName);
			Category category = categoryExists != null ? categoryExists : new Category() { CategoryId = Guid.NewGuid(), CategoryName = categoryName };
			podcastSeries.Category.Add(category);
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
			/*_applicationDbContext.PodcastEpisodes.Add(episode);
			*/
		}
		podcastSeries.Episodes = episodeList;
		/*_applicationDbContext.PodcastSeries.Add(podcastSeries);
		_applicationDbContext.SaveChangesAsync();*/
		watch.Stop();
		Console.WriteLine($"{watch.Elapsed.Seconds} seconds");
		return podcastSeries;
	}
}

