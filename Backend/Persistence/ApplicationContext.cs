using Backend.Data;
using Microsoft.EntityFrameworkCore;


namespace Backend.Persistence;

public class ApplicationContext(DbContextOptions<ApplicationContext> options  ) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Items> Items { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<Users> Users { get; set; }
 }