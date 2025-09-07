using Microsoft.AspNetCore.Components;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.UI.Questions.View;

public partial class MultipleChoicesQuestionViewComponent : ComponentBase
{
    [Parameter]
    public MultipleChoicesQuestion Question { get; set; }

    [Parameter]
    public Action DeleteAction { get; set; }

    public void AddAnswerOption(MultipleChoicesAnswer answer) => answer.Content.Add(new AnswerOption());
    public void DeleteAnswerOption(MultipleChoicesAnswer answer, AnswerOption option) => answer.Content.Remove(option);
}
