using System;
using Sophiac.Core.TestSets;

namespace Sophiac.Core.TestRuns
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

