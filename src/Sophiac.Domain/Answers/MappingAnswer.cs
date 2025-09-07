namespace Sophiac.Domain.Answers
{
	public class MappingAnswer : AnswerBase<IList<MappingAnswerOption>>
	{
		public MappingAnswer()
		{
            Content = new List<MappingAnswerOption>();
        }
	}
}

