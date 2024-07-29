using CommunityToolkit.Maui;
using DockMobile.ApiClient;
using DockMobile.ApiClient.IoC;
using Microsoft.Extensions.Logging;

namespace DockMobile.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            builder.Services.AddApiClientService(options =>
            {
                options.ApiBaseAddress = "";
            });
            builder.Services.AddSingleton<ApiClientService>();
            builder.Services.AddTransient<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
