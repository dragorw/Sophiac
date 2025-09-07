using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sophiac.Domain.Answers;

namespace Sophiac.Domain.Questions
{
    public abstract class QuestionBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public abstract int GetMaximumPoints();
    }

    [JsonDerivedType(typeof(SingleChoiceQuestion), nameof(SingleChoiceQuestion))]
    [JsonDerivedType(typeof(MultipleChoicesQuestion), nameof(MultipleChoicesQuestion))]
    [JsonDerivedType(typeof(MappingQuestion), nameof(MappingQuestion))]
    public abstract class QuestionBase<T> : QuestionBase where T : AnswerBase
    {
        public IList<T> Answers { get; set; } = new List<T>();
    }
}

