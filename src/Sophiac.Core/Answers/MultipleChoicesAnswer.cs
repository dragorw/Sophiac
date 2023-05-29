using System;
namespace Sophiac.Core.Answers
{
	public class MultipleChoicesAnswer : AnswerBase<IList<AnswerOption>>
	{
		public MultipleChoicesAnswer()
		{
			Content = new List<AnswerOption>();
		}
	}
}

