
namespace Sophiac.Domain.TestRuns
{
    public interface ITestRunsRepository
    {
        void CreateTestRun(TestRun run);
        TestRun ReadTestRun(string fileName);
        TestRun ImportTestRun(string filePath);
        IEnumerable<TestRun> ReadTestRuns();
        void DeleteTestRun(string fileName);
    }
}

