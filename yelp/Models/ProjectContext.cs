using Microsoft.EntityFrameworkCore;

namespace yelp.Models
{
    public class YelpContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public YelpContext(DbContextOptions<YelpContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BusCategory> Categories { get; set; }
        public DbSet<BusCategoryType> CategoryTypes { get; set; }
        public DbSet<BusProperties> Properties { get; set; }
        public DbSet<BusHours> Hours { get; set; }
        public DbSet<ReviewVote> ReviewVotes { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
    }
}
