using System.Text.Json.Serialization;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.Domain.TestRuns
{
	public class TestRunEntry
	{
		public TimeSpan AnswerSpan { get; set; } = TimeSpan.Zero;
		public QuestionBase? Question { get; set; } 
		public AnswerBase? Answer { get; set; }

		[JsonIgnore]
		public bool HasBeenSkipped => Answer is null;
	}
}

