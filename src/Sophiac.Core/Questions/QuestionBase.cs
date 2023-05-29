using System;
using Sophiac.Core.Answers;

namespace Sophiac.Core.Questions
{
    public abstract class QuestionBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public abstract int GetMaximumPoints();
    }

    public abstract class QuestionBase<T> : QuestionBase where T : AnswerBase
	{
        public IList<AnswerBase> Answers { get; set; } = new List<AnswerBase>();
    }
}

