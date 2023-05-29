using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sophiac.Core.TestSets;

namespace Sophiac.Core.TestRuns
{
	public class TestRunsRepository : ITestRunsRepository
    {
        private readonly string _path;
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public TestRunsRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException(path);
            var directoryPath = Path.Combine(_path, "testruns");
            Directory.CreateDirectory(directoryPath);
        }

        public void CreateTestRun(TestRun run)
        {
            // TODO Introduce validation.
            var raw = JsonConvert.SerializeObject(run, _settings);
            var name = run.FileName;
            var path = Path.Combine(_path, "testruns", name);
            // TODO Add async handling.
            // TODO Add try catch.
            File.WriteAllText(path, raw);
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
            var run = JsonConvert.DeserializeObject<TestRun>(raw, _settings);
            CreateTestRun(run);
            return run;
        }

        public TestRun ReadTestRun(string fileName)
        {
            var path = Path.Combine(_path, "testruns", fileName);
            // TODO Add async handling.
            var raw = File.ReadAllText(path);
            // TODO Add exception handling.
            return JsonConvert.DeserializeObject<TestRun>(raw, _settings);
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
                .Select(it => JsonConvert.DeserializeObject<TestRun>(it, _settings));
        }
    }
}

