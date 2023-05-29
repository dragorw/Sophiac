using System;
using Sophiac.Core.Answers;

namespace Sophiac.Core.Questions
{
    public class MultipleChoicesQuestion : QuestionBase<MultipleChoicesAnswer>
    {
        public override int GetMaximumPoints() => Answers.Where(it => it.Points > 0).Select(it => it as MultipleChoicesAnswer).Sum(it => it.Points * it.Content.Count);
    }
}

