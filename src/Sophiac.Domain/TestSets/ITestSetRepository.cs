namespace Sophiac.Domain.TestSets
{
    public interface ITestSetRepository
    {
        Task AddAsync(TestSet testSet);
        Task DeleteAsync(int id);
        Task<IEnumerable<TestSet>> GetAllAsync();
        Task<TestSet?> GetByIdAsync(int id);
        Task<TestSet?> GetByTitleAsync(string title);
        Task UpdateAsync(TestSet testSet);
    }
}
