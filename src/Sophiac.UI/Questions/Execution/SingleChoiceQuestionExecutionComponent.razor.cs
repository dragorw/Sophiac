using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using Sophiac.Domain.TestRuns;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.UI.Questions.Execution;

public partial class SingleChoiceQuestionExecutionComponent : ComponentBase
{
    [Parameter]
    public SingleChoiceQuestion Question { get; set; }

    [Parameter]
    public TestRun Run { get; set; }

    [Parameter]
    public Stopwatch Watch { get; set; }

    [Parameter]
    public Action PostAnswerAction { get; set; }

    public void RecordAnswer(SingleChoiceAnswer answer)
    {
        Watch.Stop();
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
}
