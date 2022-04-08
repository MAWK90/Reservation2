#nullable disable
using Microsoft.EntityFrameworkCore;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
    public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation2.Models.ReservationModel> Reservation { get; set; }
    }
