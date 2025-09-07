using Microsoft.EntityFrameworkCore;
using Sophiac.Domain.Settings;

namespace Sophiac.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<SophiacSettings> _settings;

        public SettingsRepository(DbContext context)
        {
            _context = context;
            _settings = _context.Set<SophiacSettings>();
        }

        public async Task<IEnumerable<SophiacSettings>> GetAllAsync()
        {
            return await _settings.ToListAsync();
        }

        public async Task<SophiacSettings?> GetByKeyAsync(string key)
        {
            return await _settings.FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task<SophiacSettings?> GetByIdAsync(Guid id)
        {
            return await _settings.FindAsync(id);
        }

        public async Task AddAsync(SophiacSettings settings)
        {
            var existing = await _settings.FirstOrDefaultAsync(s => s.Key.Equals(settings.Key));

            if (existing != null)
            {
                settings.Id = existing.Id;
                await UpdateAsync(settings);
                return;
            }

            await _settings.AddAsync(settings);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SophiacSettings settings)
        {
            var existing = await _settings.FirstOrDefaultAsync(s => s.Key.Equals(settings.Key));

            if (existing == null)
            {
                throw new InvalidOperationException("There is no such settings to update!");
            }

            // Update the tracked entity to avoid attaching a second instance
            existing.Value = settings.Value;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var setting = await GetByIdAsync(id);
            if (setting != null)
            {
                _settings.Remove(setting);
                await _context.SaveChangesAsync();
            }
        }
    }
}