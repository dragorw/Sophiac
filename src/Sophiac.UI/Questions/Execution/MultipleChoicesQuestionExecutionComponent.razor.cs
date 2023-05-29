using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Sophiac.Core;
using Sophiac.Core.TestSets;
using Sophiac.Core.TestRuns;
using Sophiac.Core.Answers;
using Sophiac.Core.Questions;

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

    protected override void OnInitialized()
    {
        PotentialOptions =
            Question
                .Answers
                .Select(it => it as MultipleChoicesAnswer)
                .SelectMany(it => it.Content)
                .Select(it => it.Content)
                .OrderBy(it => Guid.NewGuid())
                .ToList();
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

        PostAnswerAction();
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
