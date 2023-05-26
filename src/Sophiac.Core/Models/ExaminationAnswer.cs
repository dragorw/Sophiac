using System;
namespace Sophiac.Core.Models
{
    public class ExaminationAnswer
    {
        // TODO Introduce validation logic.
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
        public bool IsOptional { get; set; } = false;
    }
}

