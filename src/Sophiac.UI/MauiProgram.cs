using System.Reflection;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sophiac.Core;
using Sophiac.Core.TestRuns;
using Sophiac.Core.TestSets;
using Sophiac.UI.Settings;

namespace Sophiac.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitCore()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        var path = Path.Combine(FileSystem.Current.AppDataDirectory, "com.github.aemilivs.sophiac");
        builder.Services.AddSingleton<ITestSetsRepository>(it => new TestSetsRepository(path));
        builder.Services.AddSingleton<ITestRunsRepository>(it => new TestRunsRepository(path));
		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<OpenAIAPIProvider>();
		return builder.Build();
	}
}
