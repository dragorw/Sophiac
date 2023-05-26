using System;
using Sophiac.Core.Models;

namespace Sophiac.Core
{
	public interface IExaminationRunRepository
	{
        void CreateRun(ExaminationRun run);
        ExaminationRun ReadRun(string fileName);
        ExaminationRun ImportRun(string filePath);
        IEnumerable<ExaminationRun> ReadRuns();
        void DeleteRun(string fileName);
    }
}

