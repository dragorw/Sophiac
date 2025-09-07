using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using Sophiac.Domain.Questions;
using Sophiac.Domain.TestSets.QuestionsDelivery;

namespace Sophiac.Domain.TestSets
{
    public class TestSet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = "New Test Set";

        [NotMapped]
        [JsonIgnore]
        public string FileName =>
            Title
                .ToLower()
                .Where(it => char.IsLetterOrDigit(it) || it == ' ')
                .Select(it => it == ' ' ? '_' : it)
                .Aggregate(new StringBuilder(), (left, right) => left.Append(right))
                .Append(".json")
                .ToString();

        [NotMapped]
        [JsonIgnore]
        public bool IsComplete { get; set; } = false;

        public IList<SingleChoiceQuestion> SingleChoiceQuestions { get; set; } = new List<SingleChoiceQuestion>();
        public IList<MultipleChoicesQuestion> MultipleChoiceQuestions { get; set; } = new List<MultipleChoicesQuestion>();
        public IList<MappingQuestion> MappingQuestions { get; set; } = new List<MappingQuestion>();

        [JsonIgnore]
        [NotMapped]
        public IList<QuestionBase> Questions =>
            SingleChoiceQuestions.Cast<QuestionBase>()
                .Concat(MultipleChoiceQuestions)
                .Concat(MappingQuestions)
                .ToList();
                
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
