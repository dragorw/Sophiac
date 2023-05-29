using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Sophiac.Core;
using Sophiac.Core.TestSets;
using Sophiac.Core.TestRuns;
using Sophiac.Core.Answers;
using Sophiac.Core.Questions;

namespace Sophiac.UI.TestSets;

public partial class TestSetExecutionPage : ComponentBase
{
    [Parameter]
    public string TestSetFileName { get; set; }

    [Inject]
    public ITestSetsRepository collectionsRepository { get; set; }

    [Inject]
    public ITestRunsRepository runsRepository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private Stopwatch _watch = new Stopwatch();

    private TestSet _set = new TestSet();

    private QuestionBase _currentQuestion;

    private TestRun _run = new TestRun();

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(TestSetFileName))
            return;

        _set = collectionsRepository.ReadTestSet(TestSetFileName);
        _run.Title = _set.Title;
    }

    public void StartRun()
    {
        _currentQuestion = _set.GetNextQuestion();
        _watch.Start();
    }

    public void PostAnswerAction()
    {
        _currentQuestion = _set.GetNextQuestion();
        StateHasChanged();

        if (_currentQuestion is null)
            RecordRun(_run);
    }

    public void RecordRun(TestRun run)
    {
        _watch.Reset();
        runsRepository.CreateTestRun(_run);
        var url = $"/testruns/view/{_run.FileName}";
        manager.NavigateTo(url);
    }
}
