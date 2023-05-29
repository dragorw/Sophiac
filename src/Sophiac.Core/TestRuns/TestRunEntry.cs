using System;
using System.Text.Json.Serialization;
using Sophiac.Core.Answers;
using Sophiac.Core.Questions;

namespace Sophiac.Core.TestRuns
{
	public class TestRunEntry
	{
		public TimeSpan AnswerSpan { get; set; } = TimeSpan.Zero;
		public QuestionBase Question { get; set; }
		public AnswerBase? Answer { get; set; }

		[JsonIgnore]
		public bool HasBeenSkipped => Answer is null;
	}
}

