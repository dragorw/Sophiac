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

public partial class MappingQuestionExecutionComponent : ComponentBase
{
    [Parameter]
    public MappingQuestion Question { get; set; }

    [Parameter]
    public TestRun Run { get; set; }

    [Parameter]
    public Stopwatch Watch { get; set; }

    [Parameter]
    public Action PostAnswerAction { get; set; }

    private IList<string> PotentialLeftOptions = new List<string>();

    private IList<string> PotentialRightOptions = new List<string>();

    protected override void OnInitialized()
    {
        var potentialOptions =
            Question
                .Answers
                .Select(it => it as MappingAnswer)
                .SelectMany(it => it.Content);

        PotentialLeftOptions =
            potentialOptions
                .Select(it => it.Source)
                .OrderBy(it => Guid.NewGuid())
                .ToList();

        PotentialRightOptions =
            potentialOptions
                .Select(it => it.Destination)
                .OrderBy(it => Guid.NewGuid())
                .ToList();
    }

    public void RecordAnswer()
    {
        Watch.Stop();
        StateHasChanged();
        var options = PotentialLeftOptions.Zip(PotentialRightOptions, (left, right) => (left, right)).ToList();
        var answer =
            new MappingAnswer
            {
                Content = options.Select(it => new MappingAnswerOption { Source = it.left, Destination = it.right }).ToList(),
                Points = CalculatePoints(options)
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

    public void MoveUp(IList<string> list, int index)
    {
        if (index < 1)
            return;

        var item = list[index];
        list.RemoveAt(index--);
        list.Insert(index, item);
    }

    public void MoveDown(IList<string> list, int index)
    {
        if (index >= list.Count - 1)
            return;

        var item = list[index];
        list.RemoveAt(index++);
        list.Insert(index, item);
    }

    public int CalculatePoints(IList<(string, string)> options) =>
        options
            .Select(it =>
                Question
                    .Answers
                    .Select(answer => answer as MappingAnswer)
                    .Where(answer => answer is not null)
                    .FirstOrDefault(answer =>
                        answer
                            .Content
                            .Any(option => string.Equals(option.Source, it.Item1, StringComparison.InvariantCultureIgnoreCase) && string.Equals(option.Destination, it.Item2, StringComparison.InvariantCultureIgnoreCase))
                        )?
                    .Points ?? 0
                )
            .Sum();
}
