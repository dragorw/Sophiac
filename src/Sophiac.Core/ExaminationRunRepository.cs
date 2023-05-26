using System;
using System.Text.Json;
using Sophiac.Core.Models;

namespace Sophiac.Core
{
	public class ExaminationRunRepository : IExaminationRunRepository
	{
        private readonly string _path;

        public ExaminationRunRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException(path);
            var directoryPath = Path.Combine(_path, "runs");
            Directory.CreateDirectory(directoryPath);
        }

        public void CreateRun(ExaminationRun run)
        {
            // TODO Introduce validation.
            var raw = JsonSerializer.Serialize(run);
            var name = run.FileName;
            var path = Path.Combine(_path, "runs", name);
            // TODO Add async handling.
            // TODO Add try catch.
            File.WriteAllText(path, raw);
        }

        public void DeleteRun(string fileName)
        {
            var path = Path.Combine(_path, "runs", fileName);
            File.Delete(path);
        }

        public ExaminationRun ImportRun(string filePath)
        {
            var raw = File.ReadAllText(filePath);
            // TODO Add exception handling.
            // TODO Add validation.
            var run = JsonSerializer.Deserialize<ExaminationRun>(raw);
            CreateRun(run);
            return run;
        }

        public ExaminationRun ReadRun(string fileName)
        {
            var path = Path.Combine(_path, "runs", fileName);
            // TODO Add async handling.
            var raw = File.ReadAllText(path);
            // TODO Add exception handling.
            return JsonSerializer.Deserialize<ExaminationRun>(raw);
        }

        public IEnumerable<ExaminationRun> ReadRuns()
        {
            var path = Path.Combine(_path, "runs");
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();

            // TODO Add async handling.
            // TODO Add exception handling.
            return files
                .Where(it => string.Equals(it.Extension, ".json", StringComparison.InvariantCultureIgnoreCase))
                .Select(it => File.ReadAllText(it.FullName))
                .Select(it => JsonSerializer.Deserialize<ExaminationRun>(it));
        }
    }
}

