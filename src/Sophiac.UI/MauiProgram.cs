using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using Sophiac.Core;

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
        builder.Services.AddSingleton<IExaminationCollectionsRepository>(it => new ExaminationCollectionsRepository(path));
		// TODO Rename to plural.
        builder.Services.AddSingleton<IExaminationRunRepository>(it => new ExaminationRunRepository(path));

		return builder.Build();
	}
}
