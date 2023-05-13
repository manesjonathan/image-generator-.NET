using ImageGeneratorApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageGeneratorApi.Data;

public class ImageGeneratorApiContext : IdentityUserContext<IdentityUser>
{
    public ImageGeneratorApiContext(DbContextOptions<ImageGeneratorApiContext> options, DbSet<User> users)
        : base(options)
    {
        Users = users;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var builder = WebApplication.CreateBuilder();
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseURL"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public new DbSet<User> Users { get; set; }
}