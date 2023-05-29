using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.Questions;
using Sophiac.Core.TestSets;

namespace Sophiac.UI.TestSets;

public partial class TestSetPage : ComponentBase
{
    [Parameter]
    public string TestSetFileName { get; set; }

    [Inject]
    public ITestSetsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private TestSet _set = new TestSet();

    private IList<string> _questionTypes = new List<string>();

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(TestSetFileName))
            return;

        _set = repository.ReadTestSet(TestSetFileName);
    }

    public void OnQuestionTypeChange(ChangeEventArgs arguments, QuestionBase question)
    {
        var index = _set.Questions.IndexOf(question);
        _set.Questions.RemoveAt(index);
        
        var type = arguments.Value.ToString();

        if (type == typeof(SingleChoiceQuestion).Name)
            question =
                new SingleChoiceQuestion
                {
                    Title = question.Title,
                    Description = question.Description
                };

        if (type == typeof(MultipleChoicesQuestion).Name)
            question =
                new MultipleChoicesQuestion
                {
                    Title = question.Title,
                    Description = question.Description
                };

        if (type == typeof(MappingQuestion).Name)
            question =
                new MappingQuestion
                {
                    Title = question.Title,
                    Description = question.Description
                };

        _set.Questions.Insert(index, question);
        StateHasChanged();
    }

    public void CreateQuestion()
    {
        var question = new SingleChoiceQuestion();
        _set.Questions.Add(question);
        _questionTypes = _set.Questions.Select(it => it.GetType().Name).ToList();
    }

    public void DeleteQuestion(QuestionBase question)
    {
        _set.Questions.Remove(question);
        _questionTypes = _set.Questions.Select(it => it.GetType().Name).ToList();
        StateHasChanged();
    }

    public async Task SubmitAsync()
    {
        repository.CreateTestSet(_set);
        manager.NavigateTo("/testsets");
    }
}
