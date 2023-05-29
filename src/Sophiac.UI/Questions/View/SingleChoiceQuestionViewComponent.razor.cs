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

public partial class SingleChoiceQuestionViewComponent : ComponentBase
{
    [Parameter]
    public SingleChoiceQuestion Question { get; set; }

    [Parameter]
    public Action DeleteAction { get; set; }
}
