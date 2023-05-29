using System;
using Sophiac.Core.Answers;

namespace Sophiac.Core.Questions
{
	public class MappingQuestion : QuestionBase<MappingAnswer>
	{
        public override int GetMaximumPoints() => Answers.Where(it => it.Points > 0).Select(it => it as MappingAnswer).Sum(it => it.Points * it.Content.Count);
    }
}

