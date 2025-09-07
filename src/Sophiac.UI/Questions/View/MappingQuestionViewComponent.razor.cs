using Microsoft.AspNetCore.Components;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.UI.Questions.View;

public partial class MappingQuestionViewComponent : ComponentBase
{
    [Parameter]
    public MappingQuestion Question { get; set; }

    [Parameter]
    public Action DeleteAction { get; set; }

    public void AddAnswerOption(MappingAnswer answer) => answer.Content.Add(new MappingAnswerOption());
}
