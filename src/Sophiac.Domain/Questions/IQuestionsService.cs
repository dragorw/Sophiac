using Sophiac.Domain.TestSets;

namespace Sophiac.Domain.Questions;

public interface IQuestionsService
{
    Task CreateTestSetAsync(string testSetTitle);
    Task CreateTestSetAsync(TestSet testSet);
    Task UpdateTestSetAsync(TestSet testSet);
    Task DeleteTestSet(string testSetTitle);
    Task IntroduceQuestion(string testSetTitle, QuestionBase question);
    Task<TestSet?> ReadTestSetAsync(string testSetTitle);
    Task<IEnumerable<TestSet>> ReadTestSetsAsync();
}