using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MediaCenterCore.Models;

namespace MediaCenterCore.Data
{
    public class MediaCenterDbContext : IdentityDbContext<User>
    {
        public MediaCenterDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public DbSet<User> users { get; set; }
        public DbSet<Media> medias { get; set; }
        public DbSet<MediaAddFav> userSavedMedias { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<MediaCategory> mediaCategories { get; set; }

        public DbSet<SeriesRelation> seriesRelations { get; set; }
        public DbSet<MediaComment> MovieComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaCategory>()
                .HasKey(mc => new { mc.MediaId, mc.CategoryId });

            modelBuilder.Entity<MediaAddFav>()
                .HasKey(mc => new { mc.MediaId, mc.UserId });

            modelBuilder.Entity<MediaLike>()
                .HasKey(mc => new { mc.MediaId, mc.UserId });

            modelBuilder.Entity<MediaWatchLater>()
                .HasKey(mc => new { mc.MediaId, mc.UserId });

            modelBuilder.Entity<Media>().Property(x => x.ReleaseDate).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Media>().Property(x => x.ratesAvg).HasComputedColumnSql("[ratesSum] / [ratesCount]");
            modelBuilder.Entity<User>().Property(x => x.UserName).HasComputedColumnSql("[id]");
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique(true);

            modelBuilder.Entity<MediaLike>().HasIndex(x => new { x.UserId, x.MediaId } ).IsUnique(true);

            modelBuilder.Entity<MediaComment>().Property(x => x.CreationDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<MediaAddFav>().Property(x => x.SaveDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<MediaWatchLater>().Property(x => x.SaveDate).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Media>().HasOne<MediaVa>().WithOne(x => x.Media).HasForeignKey<MediaVa>(x=>x.MediaId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
