using Microsoft.EntityFrameworkCore;
using internship.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace internship.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItemModel> MenuItems { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTimestamps();
            HandleMenuItemUpdates();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            HandleMenuItemUpdates();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is MenuItemModel &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((MenuItemModel)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((MenuItemModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
        }

        private void HandleMenuItemUpdates()
        {
            foreach (var entry in ChangeTracker.Entries<MenuItemModel>())
            {
                if (entry.State == EntityState.Modified)
                {
                    // Sử dụng SQL thô để cập nhật mục nhập mà không cần sử dụng OUTPUT
                    var item = entry.Entity;
                    Database.ExecuteSqlRaw("UPDATE MenuItems SET ItemName = {0}, Description = {1}, Price = {2}, Available = {3}, ImageUrl = {4}, UpdatedAt = GETDATE() WHERE ItemID = {5}",
                        item.ItemName, item.Description, item.Price, item.Available, item.ImageUrl, item.ItemID);

                    entry.State = EntityState.Unchanged;
                }
            }
        }
    }
}
