using Microsoft.Extensions.Logging;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Maui;

namespace RestlessPlanet.App {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseArcGISRuntime(config => config.UseApiKey(Secrets.ArcGISApiKey))
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
