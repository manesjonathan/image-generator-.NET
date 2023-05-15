using ImageGeneratorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageGeneratorApi.Data
{
    public class ImageGeneratorApiContext : DbContext
    {
        public ImageGeneratorApiContext(DbContextOptions<ImageGeneratorApiContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}