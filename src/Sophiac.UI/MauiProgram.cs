using System.IO.Pipelines;
using System.Reflection;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Sophiac.Application.Generation;
using Sophiac.Application.Questions;
using Sophiac.Domain.Chat;
using Sophiac.Domain.Generation;
using Sophiac.Domain.Questions;
using Sophiac.Domain.Settings;
using Sophiac.Domain.TestRuns;
using Sophiac.Domain.TestSets;
using Sophiac.Infrastructure.Chat;
using Sophiac.Infrastructure.Repositories;
using Sophiac.Infrastructure.SQLite;

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
		// builder.Services.AddSingleton<ITestSetsRepository>(it => new TestSetsRepository(path));
		builder.Services.AddSingleton<ITestRunsRepository>(it => new TestRunsRepository(path));
		builder.Services.AddSingleton<ITestSetRepository, TestSetRepository>();
		builder.Services.AddSingleton(SecureStorage.Default);
		builder.Services.AddSingleton<IChatProvider, ChatProvider>();
		builder.Services.AddSingleton<IQuestionsService, QuestionsService>();
		builder.Services.AddSingleton<IGenerationService, GenerationService>();
		builder.Services.AddSingleton<QuestionsTools>();
		builder.Services.AddSingleton<ISettingsRepository, SettingsRepository>();

		var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		Directory.CreateDirectory(dbPath);
		dbPath = Path.Combine(dbPath, "sophiac.db");

		if (!File.Exists(dbPath))
			File.Create(dbPath);

		builder.Services.AddDbContext<DbContext, SophiacDbContext>(options => options.UseSqlite($"Filename={dbPath}"));

		using (var scope = builder.Build().Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<SophiacDbContext>();
			context.Database.EnsureCreated();
			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}
		}
		return builder.Build();
	}
}
