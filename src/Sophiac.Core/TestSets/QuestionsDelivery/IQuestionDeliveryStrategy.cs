using System;
using Sophiac.Core.Questions;

namespace Sophiac.Core.TestSets.QuestionsDelivery
{
	public interface IQuestionDeliveryStrategy
	{
		QuestionBase? GetNextQuestion(IList<QuestionBase> questions);
	}
}

