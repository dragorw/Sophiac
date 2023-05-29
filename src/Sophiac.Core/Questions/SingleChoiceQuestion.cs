using System;
using Sophiac.Core.Answers;

namespace Sophiac.Core.Questions
{
    public class SingleChoiceQuestion : QuestionBase<SingleChoiceAnswer>
    {
        public override int GetMaximumPoints() => Answers.Max(it => it.Points);
    }
}

