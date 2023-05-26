using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.Models;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;

namespace Sophiac.UI.Pages;

public partial class ExaminationCollectionRunView : ComponentBase
{
    [Parameter]
    public string CollectionFileName { get; set; }

    [Inject]
    public IExaminationCollectionsRepository collectionsRepository { get; set; }

    [Inject]
    public IExaminationRunRepository runsRepository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private Stopwatch _watch = new Stopwatch();

    private ExaminationCollection _collection = new ExaminationCollection();

    private ExaminationQuestion? _currentQuestion;

    private ExaminationRun _run = new ExaminationRun();

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(CollectionFileName))
            return;

        _collection = collectionsRepository.ReadCollection(CollectionFileName);
        _run.Title = _collection.Title + DateTime.UtcNow;
    }

    public void StartRun()
    {
        _currentQuestion = _collection.Next();
        _watch.Start();
    }

    public void RecordAnswer(ExaminationAnswer selectedAnswer)
    {
        _watch.Stop();
        var recordedAnswer =
            new ExaminationRunAnswer
            {
                AnswerSpan = _watch.Elapsed,
                Question = _currentQuestion,
                SelectedAnswer = selectedAnswer,
            };
        _run.Answers.Add(recordedAnswer);
        _watch.Restart();
        _currentQuestion = _collection.Next();

        if (_currentQuestion is null)
            RecordRun(_run);
    }

    public void RecordRun(ExaminationRun run)
    {
        _watch.Reset();
        runsRepository.CreateRun(_run);
        var url = $"/runs/view/{_run.FileName}";
        manager.NavigateTo(url);
    }
}
