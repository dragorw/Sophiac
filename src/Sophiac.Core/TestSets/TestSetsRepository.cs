using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sophiac.Core.TestSets;

namespace Sophiac.Core.TestSets
{
	public class TestSetsRepository : ITestSetsRepository
    {
        private readonly string _path;
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public TestSetsRepository(string path)
		{
            _path = path ?? throw new ArgumentNullException(path);
            var directoryPath = Path.Combine(_path, "testsets");
            Directory.CreateDirectory(directoryPath);
		}

        public void CreateTestSet(TestSet set)
        {
            // TODO Introduce validation.
            var raw = JsonConvert.SerializeObject(set, _settings);
            var name = set.FileName;
            var path = Path.Combine(_path, "testsets", name);
            // TODO Add async handling.
            // TODO Add try catch.
            File.WriteAllText(path, raw);
        }

        public TestSet ImportTestSet(string filePath)
        {
            var raw = File.ReadAllText(filePath);
            // TODO Add exception handling.
            // TODO Add validation.
            var set = JsonConvert.DeserializeObject<TestSet>(raw, _settings);
            CreateTestSet(set);
            return set;
        }

        public TestSet ReadTestSet(string fileName)
        {
            var path = Path.Combine(_path, "testsets", fileName);
            // TODO Add async handling.
            var raw = File.ReadAllText(path);
            // TODO Add exception handling.
            return JsonConvert.DeserializeObject<TestSet>(raw, _settings);
        }

        public IEnumerable<TestSet> ReadTestSet()
		{
            var path = Path.Combine(_path, "testsets");
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();

            // TODO Add async handling.
            // TODO Add exception handling.
            return files
                .Where(it => string.Equals(it.Extension, ".json", StringComparison.InvariantCultureIgnoreCase))
                .Select(it => File.ReadAllText(it.FullName))
                .Select(it => JsonConvert.DeserializeObject<TestSet>(it, _settings));
        }

        public void DeleteTestSet(string fileName)
        {
            var path = Path.Combine(_path, "testsets", fileName);
            File.Delete(path);
        }
	}
}

