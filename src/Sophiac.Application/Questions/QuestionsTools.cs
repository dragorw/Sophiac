using System.ComponentModel;
using ModelContextProtocol.Server;
using Sophiac.Domain.Questions;

namespace Sophiac.Application.Questions;

public class QuestionsTools
{
    private readonly IQuestionsService _service;
    public QuestionsTools(IQuestionsService service)
    {
        _service = service;
    }

    [Description("List questions that test set already contains to avoid creation of duplicates.")]
    public async Task<string[]> ListQuestionsInTestSet(string testSetTitle)
    {
        var set = await _service.ReadTestSetAsync(testSetTitle);

        if (set == null)
        {
            throw new InvalidOperationException($"Test set couldn't be found to introduce a question there!");
        }

        return set.Questions.Select(it => it.Title).ToArray();
    }

    [Description("Introduce a question with single choice answer to a test set. Returns a list of questions that test set contains.")]
    public async Task<string[]> IntroduceSingleChoiceQuestion(string testSetTitle, SingleChoiceQuestion question)
    {
        await _service.IntroduceQuestion(testSetTitle, question);
        return await ListQuestionsInTestSet(testSetTitle);
    }

    [Description("Introduce a question with multiple choice answer to a test set. Returns a list of questions that test set contains.")]
    public async Task<string[]> IntroduceMultipleChoiceQuestion(string testSetTitle, MultipleChoicesQuestion question)
    {
        await _service.IntroduceQuestion(testSetTitle, question);
        return await ListQuestionsInTestSet(testSetTitle);
    }

    [Description("Introduce a question with a mapping answer to a test set. Returns a list of questions that test set contains.")]
    public async Task<string[]> IntroduceMappingQuestion(string testSetTitle, MappingQuestion question)
    {
        await _service.IntroduceQuestion(testSetTitle, question);
        return await ListQuestionsInTestSet(testSetTitle);
    }
}