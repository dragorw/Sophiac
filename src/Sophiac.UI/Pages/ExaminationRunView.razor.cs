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

public partial class ExaminationRunView : ComponentBase
{
    [Parameter]
    public string CollectionFileName { get; set; }

    [Inject]
    public IExaminationRunRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private ExaminationRun? _run;

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(CollectionFileName))
            return;

        _run = repository.ReadRun(CollectionFileName);
    }
}
