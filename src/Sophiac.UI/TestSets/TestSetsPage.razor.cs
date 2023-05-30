using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.TestSets;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using Sophiac.UI.Settings;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using PDFIndexer.Utils;

namespace Sophiac.UI.TestSets;

public partial class TestSetsPage : ComponentBase
{
    [Inject]
    public ITestSetsRepository Repository { get; set; }

    [Inject]
    public NavigationManager Manager { get; set; }

    [Inject]
    public OpenAIAPIProvider Provider { get; set; }

    public bool IsLoading { get; set; } = false;

    private IList<TestSet> _sets;

    protected override void OnInitialized()
    {
        _sets =
            Repository
                .ReadTestSet()
                .ToList();
    }

    public void DeleteTestSet(TestSet set)
    {
        _sets.Remove(set);
        Repository.DeleteTestSet(set.FileName);
    }

    public async Task ImportTestSetAsync(CancellationToken token)
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
                    { DevicePlatform.MacCatalyst, new[] { "json" } },
                });

            var options = new PickOptions()
            {
                PickerTitle = "Please select an examination test set file",
                FileTypes = customFileType,
            };

            var file = await FilePicker.Default.PickAsync(options);
            if (file != null)
            {
                if (file.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    var set = Repository.ImportTestSet(file.FullPath);
                    _sets.Add(set);
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

    public async Task ExportTestSetAsync(TestSet set, CancellationToken token)
    {
        var permission = await CheckAndRequestStorageWritePermission();

        if (permission != PermissionStatus.Granted)
        {
            await Toast.Make("Please grant permissions to write to the storage first.").Show();
            return;
        }

        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
        var raw = JsonConvert.SerializeObject(set, settings);
        var bites = Encoding.Default.GetBytes(raw);
        using var stream = new MemoryStream(bites);
        var fileSaverResult = await FileSaver.Default.SaveAsync(set.FileName, stream, token);
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
            await Toast.Make("This permission is required in order to import examination test sets and test runs.").Show();
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
            await Toast.Make("This permission is required in order to export examination test sets and test runs.").Show();
        }

        status = await Permissions.RequestAsync<Permissions.StorageWrite>();

        return status;
    }

    private async Task ParsePDFAsync()
    {
        if (string.IsNullOrEmpty(Provider.APIKey))
        {
            await Toast.Make("API Key is not set.").Show();
            return;
        }

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
                    { DevicePlatform.iOS, new[] { "public.pdf" } },
                    { DevicePlatform.Android, new[] { "application/pdf" } },
                    { DevicePlatform.WinUI, new[] { ".pdf", ".pdf" } },
                    { DevicePlatform.Tizen, new[] { "application/pdf" } },
                    { DevicePlatform.MacCatalyst, new[] { "pdf" } },
                });

            var options = new PickOptions()
            {
                PickerTitle = "Please select a pdf to parse for questions",
                FileTypes = customFileType,
            };

            var file = await FilePicker.Default.PickAsync(options);
            IsLoading = true;
            StateHasChanged();
            if (file != null)
            {

                var extractor = new TextExtractor();
                var text = extractor.ExtractFullText(file.FullPath);
                var pages =
                    text
                        .Split(' ')
                        .Select((word, index) => new { word, index })
                        .GroupBy(x => x.index / 500)
                        .Select(group => string.Join(" ", group.Select(x => x.word)))
                        .ToList();
                await Toast.Make("Processing PDF might take several minutes.").Show();
                var raw = await Provider.PredictPayloadAsync(pages);
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var set = JsonConvert.DeserializeObject<TestSet>(raw, settings);
                if (set is null)
                {
                    await Toast.Make("Processing response from OpenAI failed. Please, try again.").Show();
                    return;
                }
                Repository.CreateTestSet(set);
                _sets.Add(set);
                Toast.Make($"Successfully parsed {set.Title}");
            }
            else
            {
                await Toast.Make("Couldn't import the file!").Show();
            }
        }
        catch (Exception exception)
        {
            await Toast.Make($"Something went seriously wrong. {exception.Message}", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
        }
        finally
        {
            IsLoading = false;
        }
    }
}
