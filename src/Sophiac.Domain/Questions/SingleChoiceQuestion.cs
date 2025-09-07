using Sophiac.Domain.Answers;

namespace Sophiac.Domain.Questions
{
    public class SingleChoiceQuestion : QuestionBase<SingleChoiceAnswer>
    {
        public override int GetMaximumPoints() => Answers.Max(it => it.Points);
    }
}

