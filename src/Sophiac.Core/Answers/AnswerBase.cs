using System;
namespace Sophiac.Core.Answers
{
	public abstract class AnswerBase
	{
		public int Points { get; set; } = 0;
	}

	public abstract class AnswerBase<T> : AnswerBase
	{
		public T Content { get; set; }
	}
}

