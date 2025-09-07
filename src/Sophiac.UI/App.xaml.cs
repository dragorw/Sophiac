namespace Sophiac.UI;

public partial class App : Microsoft.Maui.Controls.Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override void OnStart()
	{
		var path = Path.Combine(FileSystem.Current.AppDataDirectory, "com.github.aemilivs.sophiac");
		var info = Directory.CreateDirectory(path);
		base.OnStart();
	}
	
	protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new MainPage());
    }
}
