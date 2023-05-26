using System;
using Sophiac.Core.Models;

namespace Sophiac.Core
{
	public interface IExaminationCollectionsRepository
	{
        void CreateCollection(ExaminationCollection collection);
        ExaminationCollection ReadCollection(string fileName);
        ExaminationCollection ImportCollection(string filePath);
        IEnumerable<ExaminationCollection> ReadCollections();
        void DeleteCollection(string fileName);
    }
}

