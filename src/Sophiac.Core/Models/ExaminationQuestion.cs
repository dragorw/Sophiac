using System;
namespace Sophiac.Core.Models
{
	public class ExaminationQuestion
	{
        // TODO Introduce validation logic.
        public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public IList<ExaminationAnswer> Answers { get; set; } = new List<ExaminationAnswer>();

		public void AddAnswer()
		{
			var answer = new ExaminationAnswer();
			Answers.Add(answer);
		}

		public void RemoveAnswer(ExaminationAnswer answer)
		{
			Answers.Remove(answer);
		}

		public bool Verify(string answer) => Answers.Any(it => Verify(answer));
	}
}

