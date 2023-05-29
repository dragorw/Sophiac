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
