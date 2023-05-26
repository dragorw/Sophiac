using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Sophiac.Core.Models;

namespace Sophiac.Core
{
	public class ExaminationCollectionsRepository : IExaminationCollectionsRepository
    {
        private readonly string _path;

		public ExaminationCollectionsRepository(string path)
		{
            _path = path ?? throw new ArgumentNullException(path);
            var directoryPath = Path.Combine(_path, "collections");
            Directory.CreateDirectory(directoryPath);
		}

        public void CreateCollection(ExaminationCollection collection)
        {
            // TODO Introduce validation.
            var raw = JsonSerializer.Serialize(collection);
            var name = collection.FileName;
            var path = Path.Combine(_path, "collections", name);
            // TODO Add async handling.
            // TODO Add try catch.
            File.WriteAllText(path, raw);
        }

        public ExaminationCollection ImportCollection(string filePath)
        {
            var raw = File.ReadAllText(filePath);
            // TODO Add exception handling.
            // TODO Add validation.
            var collection = JsonSerializer.Deserialize<ExaminationCollection>(raw);
            CreateCollection(collection);
            return collection;
        }

        public ExaminationCollection ReadCollection(string fileName)
        {
            var path = Path.Combine(_path, "collections", fileName);
            // TODO Add async handling.
            var raw = File.ReadAllText(path);
            // TODO Add exception handling.
            return JsonSerializer.Deserialize<ExaminationCollection>(raw);
        }

        public IEnumerable<ExaminationCollection> ReadCollections()
		{
            var path = Path.Combine(_path, "collections");
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();

            // TODO Add async handling.
            // TODO Add exception handling.
            return files
                .Where(it => string.Equals(it.Extension, ".json", StringComparison.InvariantCultureIgnoreCase))
                .Select(it => File.ReadAllText(it.FullName))
                .Select(it => JsonSerializer.Deserialize<ExaminationCollection>(it));
        }

        public void DeleteCollection(string fileName)
        {
            var path = Path.Combine(_path, "collections", fileName);
            File.Delete(path);
        }
	}
}

