using System.Text.Json.Serialization;

namespace Sophiac.Domain.Answers
{
	public abstract class AnswerBase
	{
		public int Points { get; set; } = 0;
	}

	[JsonDerivedType(typeof(SingleChoiceAnswer), nameof(SingleChoiceAnswer))]
	[JsonDerivedType(typeof(MultipleChoicesAnswer), nameof(MultipleChoicesAnswer))]
	[JsonDerivedType(typeof(MappingAnswer), nameof(MappingAnswer))]
	public abstract class AnswerBase<T> : AnswerBase
	{
		public T Content { get; set; }
	}
}


