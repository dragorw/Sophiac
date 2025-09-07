using Microsoft.AspNetCore.Components;
using Sophiac.Domain.TestRuns;

namespace Sophiac.UI.Runs;

public partial class TestRunPage : ComponentBase
{
    [Parameter]
    public string TestRunFileName { get; set; }

    [Inject]
    public ITestRunsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private TestRun? _run;

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(TestRunFileName))
            return;

        _run = repository.ReadTestRun(TestRunFileName);
    }
}
