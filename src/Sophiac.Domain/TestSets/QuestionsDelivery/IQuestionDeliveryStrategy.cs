using Sophiac.Domain.Questions;

namespace Sophiac.Domain.TestSets.QuestionsDelivery
{
	public interface IQuestionDeliveryStrategy
	{
		QuestionBase? GetNextQuestion(IList<QuestionBase> questions);
	}
}

