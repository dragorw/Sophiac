using System;
namespace Sophiac.Core.Answers
{
	public class MappingAnswer : AnswerBase<IList<MappingAnswerOption>>
	{
		public MappingAnswer()
		{
            Content = new List<MappingAnswerOption>();
        }
	}
}

