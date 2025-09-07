using System.Text.Json;
using Microsoft.AspNetCore.Components;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using Sophiac.Domain.TestRuns;

namespace Sophiac.UI.Runs;

public partial class TestRunsPage : ComponentBase
{
    [Inject]
    public ITestRunsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private IList<TestRun> _runs;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _runs = repository.ReadTestRuns().ToList();
        }
        catch (Exception e)
        {
            await Toast.Make($"Couldn't load test runs: {e.Message}").Show();
            throw;
        }
    }

    public void DeleteTestRun(TestRun run)
    {
        _runs.Remove(run);
        repository.DeleteTestRun(run.FileName);
    }

    public async Task ImportTestRunAsync(CancellationToken token)
    {
        var permission = await CheckAndRequestStorageReadPermission();

        if (permission != PermissionStatus.Granted)
        {
            await Toast.Make("Please grant permissions to read the storage first.").Show();
            return;
        }

        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.WinUI, new[] { ".json", ".json" } },
                    { DevicePlatform.Tizen, new[] { "application/json" } },
                    { DevicePlatform.macOS, new[] { "public.json" } },
                });

            var options = new PickOptions()
            {
                PickerTitle = "Please select an examination run file",
                FileTypes = customFileType,
            };

            var file = await FilePicker.Default.PickAsync(options);
            if (file != null)
            {
                if (file.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    var run = repository.ImportTestRun(file.FullPath);
                    _runs.Add(run);
                }
            }
            else
            {
                await Toast.Make("Couldn't import the file!").Show(token);
            }
        }
        catch (Exception ex)
        {
            await Toast.Make("Something went seriously wrong.").Show(token);
        }
    }

    public async Task ExportTestRunAsync(TestRun run, CancellationToken token)
    {
        var permission = await CheckAndRequestStorageWritePermission();

        if (permission != PermissionStatus.Granted)
        {
            await Toast.Make("Please grant permissions to write to the storage first.").Show();
            return;
        }


        var raw = JsonSerializer.Serialize(run, new JsonSerializerOptions() { WriteIndented = true });
        var bites = Encoding.Default.GetBytes(raw);
        using var stream = new MemoryStream(bites);
        var fileSaverResult = await FileSaver.Default.SaveAsync(run.FileName, stream, token);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(token);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(token);
        }
    }

    private async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

        if (status == PermissionStatus.Granted)
            return status;

        if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
        {
            await Toast.Make("This permission is required in order to import examination collections and examination results.").Show();
        }

        status = await Permissions.RequestAsync<Permissions.StorageRead>();

        return status;
    }

    private async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

        if (status == PermissionStatus.Granted)
            return status;

        if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
        {
            await Toast.Make("This permission is required in order to export examination collections and examination results.").Show();
        }

        status = await Permissions.RequestAsync<Permissions.StorageWrite>();

        return status;
    }
}
