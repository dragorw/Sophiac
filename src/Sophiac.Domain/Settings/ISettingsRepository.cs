namespace Sophiac.Domain.Settings;

public interface ISettingsRepository
    {
        Task AddAsync(SophiacSettings settings);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<SophiacSettings>> GetAllAsync();
        Task<SophiacSettings?> GetByIdAsync(Guid id);
        Task<SophiacSettings?> GetByKeyAsync(string key);
        Task UpdateAsync(SophiacSettings settings);
    }