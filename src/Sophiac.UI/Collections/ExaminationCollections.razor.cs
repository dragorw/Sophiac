using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.Models;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;

namespace Sophiac.UI.Collections;

public partial class ExaminationCollections : ComponentBase
{
    [Inject]
    public IExaminationCollectionsRepository repository { get; set; }

    [Inject]
    public NavigationManager manager { get; set; }

    private IList<ExaminationCollection> _collections;

    protected override void OnInitialized()
    {
        _collections =
            repository
                .ReadCollections()
                .ToList();
    }

    public void DeleteCollection(ExaminationCollection collection)
    {
        _collections.Remove(collection);
        repository.DeleteCollection(collection.FileName);
    }

    public async Task ImportCollectionAsync(CancellationToken token)
    {
        var permission = await CheckAndRequestStorageReadPermission();

        if (permission != PermissionStatus.Granted)
            return;

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
                PickerTitle = "Please select an examination collection file",
                FileTypes = customFileType,
            };

            var file = await FilePicker.Default.PickAsync(options);
            if (file != null)
            {
                if (file.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    var collection = repository.ImportCollection(file.FullPath);
                    _collections.Add(collection);
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

    public async Task ExportCollectionAsync(ExaminationCollection collection, CancellationToken token)
    {
        var permission = await CheckAndRequestStorageWritePermission();

        if (permission != PermissionStatus.Granted)
            return;

        var raw = JsonSerializer.Serialize(collection);
        var bites = Encoding.Default.GetBytes(raw);
        using var stream = new MemoryStream(bites);
        var fileSaverResult = await FileSaver.Default.SaveAsync(collection.FileName, stream, token);
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
