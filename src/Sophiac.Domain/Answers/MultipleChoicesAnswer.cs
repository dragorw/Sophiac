namespace Sophiac.Domain.Answers
{
	public class MultipleChoicesAnswer : AnswerBase<IList<AnswerOption>>
	{
		public MultipleChoicesAnswer()
		{
			Content = new List<AnswerOption>();
		}
	}
}

