using System;
using System.Text;
using System.Text.Json.Serialization;
using Sophiac.Core.Questions;
using Sophiac.Core.TestSets.QuestionsDelivery;

namespace Sophiac.Core.TestSets
{
	public class TestSet
	{
        // TODO Introduce validation logic.
        public string Title { get; set; } = "New Test Set";

		[JsonIgnore]
		public string FileName =>
			Title
				.ToLower()
                .Where(it => char.IsLetterOrDigit(it) || it == ' ')
                .Select(it => it == ' ' ? '_' : it)
                .Aggregate(new StringBuilder(), (left, right) => left.Append(right))
                .Append(".json")
                .ToString();

        public IList<QuestionBase> Questions { get; set; } = new List<QuestionBase>();
        public QuestionDeliveryStrategies Strategy { get; set; } = QuestionDeliveryStrategies.SequentialDelivery;

        public QuestionBase? GetNextQuestion()
        {
            IQuestionDeliveryStrategy strategy;
            switch (Strategy)
            {
                case QuestionDeliveryStrategies.RandomDelivery:
                    strategy = new RandomQuestionDeliveryStrategy();
                    return strategy.GetNextQuestion(Questions);
                case QuestionDeliveryStrategies.SequentialDelivery:
                default:
                    strategy = new SequentialQuestionDeliveryStrategy();
                    return strategy.GetNextQuestion(Questions);
            }
        }
    }
}
