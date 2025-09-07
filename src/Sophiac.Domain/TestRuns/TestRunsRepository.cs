using System.Text.Json;

namespace Sophiac.Domain.TestRuns
{
	public class TestRunsRepository : ITestRunsRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            
        };

        public TestRunsRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException(path);
            var directoryPath = Path.Combine(_path, "testruns");
            Directory.CreateDirectory(directoryPath);
        }

        public void CreateTestRun(TestRun run)
        {
            // TODO Introduce validation.
            var raw = JsonSerializer.Serialize(run, _options);
            var name = run.FileName;
            var path = Path.Combine(_path, "testruns", name);
            // TODO Add async handling.
            // TODO Add try catch.
            try
            {
                File.WriteAllText(path, raw);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void DeleteTestRun(string fileName)
        {
            var path = Path.Combine(_path, "testruns", fileName);
            File.Delete(path);
        }

        public TestRun ImportTestRun(string filePath)
        {
            var raw = File.ReadAllText(filePath);
            // TODO Add exception handling.
            // TODO Add validation.
            var run = JsonSerializer.Deserialize<TestRun>(raw, _options);
            CreateTestRun(run);
            return run;
        }

        public TestRun ReadTestRun(string fileName)
        {
            var path = Path.Combine(_path, "testruns", fileName);
            // TODO Add async handling.
            var raw = File.ReadAllText(path);
            // TODO Add exception handling.
            return JsonSerializer.Deserialize<TestRun>(raw, _options);
        }

        public IEnumerable<TestRun> ReadTestRuns()
        {
            var path = Path.Combine(_path, "testruns");
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();

            // TODO Add async handling.
            // TODO Add exception handling.
            return files
                .Where(it => string.Equals(it.Extension, ".json", StringComparison.InvariantCultureIgnoreCase))
                .Select(it => File.ReadAllText(it.FullName))
                .Select(it => JsonSerializer.Deserialize<TestRun>(it, _options));
        }
    }
}

