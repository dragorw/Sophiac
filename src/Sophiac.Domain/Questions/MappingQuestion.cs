using Sophiac.Domain.Answers;

namespace Sophiac.Domain.Questions
{
	public class MappingQuestion : QuestionBase<MappingAnswer>
	{
        public override int GetMaximumPoints() => Answers.Where(it => it.Points > 0).Sum(it => it.Points * it.Content.Count);
    }
}

