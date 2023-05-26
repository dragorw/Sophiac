using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.Models;

namespace Sophiac.UI.Pages;

public partial class ExaminationCollectionView : ComponentBase
{
    [Parameter]
    public string CollectionFileName { get; set; }

    [Inject]
    public IExaminationCollectionsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private ExaminationCollection _collection = new ExaminationCollection();

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(CollectionFileName))
            return;

        _collection = repository.ReadCollection(CollectionFileName);
    }

    public void AddQuestion()
    {
        var question = new ExaminationQuestion();
        _collection.Questions.Add(question);
    }

    public void RemoveQuestion(ExaminationQuestion question)
    {
        _collection.Questions.Remove(question);
    }

    public async Task SubmitAsync()
    {
        repository.CreateCollection(_collection);
        manager.NavigateTo("/collections");
    }
}
