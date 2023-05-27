using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sophiac.Core.Models
{
	public class ExaminationRun
	{
		public string Title { get; set; } = string.Empty;

        public DateTime Time { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public string FileName =>
            Title
                .ToLower()
                .Where(it => char.IsLetterOrDigit(it) || it == ' ')
                .Select(it => it == ' ' ? '_' : it)
                .Aggregate(new StringBuilder(), (left, right) => left.Append(right))
                .Append(Time.ToLocalTime().ToString("yyyyMMddHHmmss"))
                .Append(".json")
                .ToString();

        public IList<ExaminationRunAnswer> Answers { get; set; } = new List<ExaminationRunAnswer>();
	}
}

