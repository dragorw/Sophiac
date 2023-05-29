using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.TestSets;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Sophiac.Core.TestRuns;

namespace Sophiac.UI.Runs;

public partial class TestRunPage : ComponentBase
{
    [Parameter]
    public string TestSetFileName { get; set; }

    [Inject]
    public ITestRunsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private TestRun? _run;

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(TestSetFileName))
            return;

        _run = repository.ReadTestRun(TestSetFileName);
    }
}
