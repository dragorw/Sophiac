using System;
using System.Text.Json.Serialization;

namespace Sophiac.Core.Models
{
	public class ExaminationRunAnswer
	{
		public TimeSpan AnswerSpan { get; set; } = TimeSpan.Zero;
		public ExaminationQuestion Question { get; set; } = new ExaminationQuestion();
		public ExaminationAnswer? SelectedAnswer { get; set; }

		[JsonIgnore]
		public bool HasBeenSkipped => SelectedAnswer is null;
	}
}

