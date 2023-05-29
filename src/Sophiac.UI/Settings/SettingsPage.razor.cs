using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Sophiac.Core;
using Sophiac.Core.TestSets;
using System.Text;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Sophiac.UI.Settings;

public partial class SettingsPage : ComponentBase
{
    [Inject]
    public ISecureStorage Storage { get; set; }

    [Inject]
    public IConfiguration Configuration { get; set; }

    [Inject]
    public OpenAIAPIProvider Provider { get; set; }

    private string _key = string.Empty;

    protected async override Task OnInitializedAsync()
    {
        _key = await Storage.GetAsync("openai-api-key") ?? Provider.APIKey;
    }

    private async Task SaveKeyAsync(ChangeEventArgs arguments)
    {
        _key = arguments.Value.ToString();

        try
        {
            await Storage.SetAsync("openai-api-key", _key);
        }
        catch (Exception exception)
        {
            await Toast.Make($"Failed to save the key permamently: {exception.Message}").Show();
        }
        finally
        {
            Provider.APIKey = _key;
        }
    }
}
