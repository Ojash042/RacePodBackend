using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using RacePodBackend.Model;

namespace RacePodBackend.Services; 

public class FeedReader {
	private string[] rssList = new string[]{"https://feeds.libsyn.com/65267/rss","https://feeds.megaphone.fm/darknetdiaries"};
	private readonly ILogger<FeedReader> _logger;

	private XNamespace nsItunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
	private XNamespace nsContent = "http://purl.org/rss/1.0/modules/content/";
	
	public FeedReader(ILogger<FeedReader> logger) {
		_logger = logger;
	}

	/*public PodcastSeries WorkingReadFeed() {
		PodcastSeries podcastSeries = new PodcastSeries();
		List<PodcastEpisode> episodeList = new List<PodcastEpisode>();
		SyndicationFeed feed = null;
		string nsItunes ="http://www.itunes.com/dtds/podcast-1.0.dtd";
		string nsContent = "http://purl.org/rss/1.0/modules/content/";
		try {
			using (var reader = XmlReader.Create(rssList[0])) {
				feed = SyndicationFeed.Load(reader);
			}
		}
		catch {
			// ignored
		}
		
		if (feed != null) {
			podcastSeries.Title = feed.Title.Text;
			podcastSeries.Author = feed.Authors.FirstOrDefault().Name;
			foreach (var item in feed.Items) {
			}
		}
		
		return podcastSeries;
	}
*/
	/*public List<PodcastEpisode> requestEpisode() {
		List<PodcastEpisode> episodeList = new List<PodcastEpisode>();
		var xml = XDocument.Load(rssList[1]);
		var channel = xml.Descendants("channel");

		for (int i = 0; i < i + 10 && i < xml.Descendants("item").Count(); i++) {
			PodcastEpisode episode
		}
		
		return episodeList;
	}*/
	
	public PodcastSeries requestPodcastDetails() {
		//Console.WriteLine(WorkingReadFeed().Author);
		//return WorkingReadFeed();
		PodcastSeries podcastSeries = new PodcastSeries();
		var xml = XDocument.Load(rssList[1]);
		var channel = xml.Descendants("channel");
		podcastSeries.Author = channel.Elements(nsItunes + "author").FirstOrDefault()?.Value ?? "";
		podcastSeries.Keywords = channel.Elements(nsItunes + "keywords").FirstOrDefault()?.Value ?? "";
		podcastSeries.Title = channel.Elements("title").FirstOrDefault()?.Value ?? "";
		//podcastSeries.PublishedDate = DateTime.ParseExact(channel.Elements("pubDate").FirstOrDefault().Value);
		podcastSeries.Url = channel.Elements("link").FirstOrDefault()?.Value ?? "";
		podcastSeries.language = channel.Elements("language").FirstOrDefault().Value ?? "";
		if (channel.Elements("copyright").First() is XCData) {
			podcastSeries.copyright = ((XCData)(channel.Elements("copyright").First().FirstNode)).Value;
		}
		else {
			podcastSeries.copyright = channel.Elements("copyright").FirstOrDefault().Value ?? "";
		}

		foreach (var cat in channel.Elements(nsItunes + "category")) {
			podcastSeries.category.Add(cat.Attribute("text").Value);	
		};

		if (channel.Elements("desciption").FirstOrDefault() is XCData) {
			podcastSeries.Description = ((XCData)(xml.Element("description").FirstNode)).Value;
		}
		
		else {
			podcastSeries.Description = channel.Elements("description").FirstOrDefault().Value ?? "";
		}
		podcastSeries.Image = channel.Elements(nsItunes + "image").FirstOrDefault().Attribute("href").Value;

		List<PodcastEpisode> episodeList = new List<PodcastEpisode>();
		foreach (var item in xml.Descendants("item")) {
			PodcastEpisode episode = new PodcastEpisode();
			episode.Title = item.Element("title").Value;
			episode.PublishedDate = DateTime.Parse(item.Element("pubDate").Value);
			episode.ImageUrl = item.Element(nsItunes + "image").Value;


			episode.EpisodeDescription = item.Element(nsContent + "encoded").Value;

			episode.AudioType = item.Element("enclosure").Attribute("type").Value;
			episode.AudioSource = item.Element("enclosure").Attribute("url").Value;
			episodeList.Add(episode);
		}

		podcastSeries.Episodes = episodeList;
		return podcastSeries;
	}
}

