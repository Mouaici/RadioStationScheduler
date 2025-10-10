using Microsoft.EntityFrameworkCore;

namespace RadioStationScheduler.Data
{
    public class RadioStationContext : DbContext
    {
        public RadioStationContext(DbContextOptions<RadioStationContext> options)
            : base(options)
        {
        }

        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }

   
    }
}
 