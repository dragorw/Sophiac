using Sophiac.Domain.Questions;

namespace Sophiac.Domain.TestSets.QuestionsDelivery
{
	public class RandomQuestionDeliveryStrategy : IQuestionDeliveryStrategy
	{
        private readonly Random _random = new Random();

        public QuestionBase? GetNextQuestion(IList<QuestionBase> questions)
        {
            if (questions.Count < 1)
                    return null;

            var index = _random.Next(0, questions.Count);
            var question = questions[index];
            questions.RemoveAt(index);
            return question;
        }
    }
}

