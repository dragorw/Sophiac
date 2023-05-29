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

namespace Sophiac.UI.Questions.View;

public partial class MappingQuestionViewComponent : ComponentBase
{
    [Parameter]
    public MappingQuestion Question { get; set; }

    [Parameter]
    public Action DeleteAction { get; set; }

    public void AddAnswerOption(MappingAnswer answer) => answer.Content.Add(new MappingAnswerOption());
}
