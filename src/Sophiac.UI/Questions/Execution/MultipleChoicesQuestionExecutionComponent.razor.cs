using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using Sophiac.Domain.TestRuns;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.UI.Questions.Execution;

public partial class MultipleChoicesQuestionExecutionComponent : ComponentBase
{
    [Parameter]
    public MultipleChoicesQuestion Question { get; set; }

    [Parameter]
    public TestRun Run { get; set; }

    [Parameter]
    public Stopwatch Watch { get; set; }

    [Parameter]
    public Action PostAnswerAction { get; set; }

    private IList<string> PotentialOptions = new List<string>();

    private ISet<string> SelectedOptions = new HashSet<string>();

    protected override void OnParametersSet()
    {
        PotentialOptions =
            Question
                .Answers
                .Select(it => it as MultipleChoicesAnswer)
                .SelectMany(it => it.Content)
                .Select(it => it.Content)
                .Distinct()
                .OrderBy(it => Guid.NewGuid())
                .ToList();
        StateHasChanged();

        base.OnParametersSet();
    }

    public void RecordAnswer()
    {
        Watch.Stop();
        var answer =
            new MultipleChoicesAnswer
            {
                Content = SelectedOptions.Select(it => new AnswerOption { Content = it }).ToList(),
                Points = CalculatePoints(SelectedOptions)
            };
        var entry =
            new TestRunEntry
            {
                AnswerSpan = Watch.Elapsed,
                Question = Question,
                Answer = answer,
            };
        Run.Entries.Add(entry);
        Watch.Restart();

        PotentialOptions = Enumerable.Empty<string>().ToList();
        SelectedOptions = Enumerable.Empty<string>().ToHashSet();
        
        PostAnswerAction();
        StateHasChanged();
    }

    public int CalculatePoints(ISet<string> options) =>
        options
            .Select(it =>
                Question
                    .Answers
                    .Select(answer => answer as MultipleChoicesAnswer)
                    .Where(answer => answer is not null)
                    .FirstOrDefault(answer =>
                        answer
                            .Content
                            .Any(option => string.Equals(option.Content, it, StringComparison.InvariantCultureIgnoreCase))
                        )
                    .Points
                )
            .Sum();

    public void ToggleOption(string option)
    {
        var isSelected = SelectedOptions.Add(option);

        if (isSelected == false)
            SelectedOptions.Remove(option);
    }

    public string GetOptionClassIndicator(string option) => SelectedOptions.Contains(option) ? "answer-option-selected" : "";
}
