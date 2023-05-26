using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sophiac.Core.Models
{
	public class ExaminationCollection
	{
        // TODO Introduce validation logic.
        public string Title { get; set; } = string.Empty;

		[JsonIgnore]
		public string FileName =>
			Title
				.ToLower()
                .Where(it => char.IsLetterOrDigit(it) || it == ' ')
                .Select(it => it == ' ' ? '_' : it)
                .Aggregate(new StringBuilder(), (left, right) => left.Append(right))
                .Append(".json")
                .ToString();

        public IList<ExaminationQuestion> Questions { get; set; } = new List<ExaminationQuestion>();

		public ExaminationQuestion? Next()
		{
            // TODO Introduce validation logic.
			var count = Questions.Count;

			if (count > 0)
			{
				var random = new Random();
                var index = random.Next(0, count);
				var question = Questions[index];
				Questions.RemoveAt(index);

				return question;
            }
			else
			{
				return null;
			}
        }
    }
}

