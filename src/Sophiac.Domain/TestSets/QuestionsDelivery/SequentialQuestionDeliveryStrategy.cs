using Sophiac.Domain.Questions;

namespace Sophiac.Domain.TestSets.QuestionsDelivery
{
    public class SequentialQuestionDeliveryStrategy : IQuestionDeliveryStrategy
    {
        public QuestionBase? GetNextQuestion(IList<QuestionBase> questions)
        {
            if (questions.Count < 1)
                return null;

            var question = questions[0];
            questions.RemoveAt(0);
            return question;
        }
    }
}

