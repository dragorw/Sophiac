
using Microsoft.AspNetCore.Components;
using Sophiac.Domain.Questions;

namespace Sophiac.UI.Questions.View;

public partial class SingleChoiceQuestionViewComponent : ComponentBase
{
    [Parameter]
    public SingleChoiceQuestion Question { get; set; }

    [Parameter]
    public Action DeleteAction { get; set; }
}
