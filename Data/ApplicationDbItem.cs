using Microsoft.EntityFrameworkCore;
using store_api.Models;

namespace store_api.Data
{
    public class ApplicationDbItem : DbContext
    {
        public ApplicationDbItem(DbContextOptions<ApplicationDbItem> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        public override int SaveChanges()
        {
            updateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            updateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void updateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Item && (
                        e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var item = (Item)entityEntry.Entity;
                if (entityEntry.State == EntityState.Added)
                {
                    item.CreatedAt = DateTime.Now;
                }
                    item.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}