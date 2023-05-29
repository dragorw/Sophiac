using System;

namespace Sophiac.Core.TestSets
{
	public interface ITestSetsRepository
	{
        void CreateTestSet(TestSet set);
        TestSet ReadTestSet(string fileName);
        TestSet ImportTestSet(string filePath);
        IEnumerable<TestSet> ReadTestSet();
        void DeleteTestSet(string fileName);
    }
}

