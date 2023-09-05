using Microsoft.Extensions.Logging;
using System.Reflection;
using WpfAppTemplate.Core.Model.Interfaces;
using WpfAppTemplate.Core.Model;
using WpfAppTemplate.Maui.Model;

namespace WpfAppTemplate.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }


    //https://github.com/dotnet/maui/issues/5507
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {

        var currentAssembly = Assembly.GetExecutingAssembly();
        var otherAssembly = Assembly.GetAssembly(typeof(ViewModelBase));

        // Transient objects lifetime services are created each time they are requested.
        // This lifetime works best for lightweight, stateless services.
        foreach (var type in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page)) || e.IsSubclassOf(typeof(ViewModelBase))))
        {
            builder.Services.AddTransient(type.AsType());
        }

        foreach (var type in otherAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(ViewModelBase))))
        {
            builder.Services.AddTransient(type.AsType());
        }

        // Oke, other services.
        builder.Services.AddSingleton<INavigationService, FakeNavigationService>(); // change FakeNavigationService when proper version is implemented!
        builder.Services.AddTransient<Core.Model.Config.Config>();

        return builder;
    }
}
