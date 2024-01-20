
using Microsoft.EntityFrameworkCore;
using RacePodBackend.Model;

namespace RacePodBackend.Data; 

public class ApplicationDbContext : DbContext{
	public DbSet<PodcastSeries> PodcastSeries { get; set; }
	public DbSet<PodcastEpisode> PodcastEpisodes { get; set; }
	public DbSet<Category> Categories { get; set; }
	//public DbSet<PodcastSeriesCategory> PodcastCategories { get; set; }
	public DbSet<UserData> UserData { get; set; }
	
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {

		modelBuilder.Entity<PodcastSeries>()
			.HasMany(podcastSeries => podcastSeries.Episodes)
			.WithOne(podcastEpisode => podcastEpisode.PodcastSeries)
			.HasForeignKey(podcastEpisode => podcastEpisode.PodcastSeriesId)
			.IsRequired();

		modelBuilder.Entity<UserData>()
			.HasOne(userData => userData.PodcastEpisode)
			.WithOne(podcastEpisode => podcastEpisode.UserData)
			.HasForeignKey<UserData>(userData => userData.PodcastEpisodeId)
			.IsRequired();

	}
	
	
}