using System.Reactive;
using Akavache;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using CSharpMarkupPeopleInSpaceMaui.Alerts;
using CSharpMarkupPeopleInSpaceMaui.Apis;
using CSharpMarkupPeopleInSpaceMaui.HotReload;
using CSharpMarkupPeopleInSpaceMaui.Navigation;
using CSharpMarkupPeopleInSpaceMaui.Network;
using CSharpMarkupPeopleInSpaceMaui.Reactive;
using CSharpMarkupPeopleInSpaceMaui.Repositories;
using CSharpMarkupPeopleInSpaceMaui.ViewModels;
using CSharpMarkupPeopleInSpaceMaui.Views;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Refit;

namespace CSharpMarkupPeopleInSpaceMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Akavache.Registrations.Start("PeopleInSpace");

        RxApp.DefaultExceptionHandler = new AnonymousObserver<Exception>(ex =>
        {
            App.Current?.MainPage?.DisplayAlert("Error", ex.Message, "OK");
        });
        
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        builder.UseMauiCommunityToolkit(options =>
        {
            options.SetShouldSuppressExceptionsInConverters(false);
            options.SetShouldSuppressExceptionsInBehaviors(false);
            options.SetShouldSuppressExceptionsInAnimations(false);
        });

      
        builder.Services.AddSingleton<IBlobCache>(BlobCache.LocalMachine);
        
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddScoped<MainPageViewModel>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddScoped<DetailPageViewModel>();
        builder.Services.AddSingleton<ICrewRepository, CrewRepository>();
        builder.Services.AddSingleton<ISchedulerProvider, SchedulerProvider>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IUserAlerts, UserAlerts>();
        builder.Services.AddRefitClient<ISpaceXApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.spacexdata.com/v4"));
        builder.Services.AddSingleton<IConnectivity>(provider => Connectivity.Current);
        builder.Services.AddSingleton<INetworkStatusObserver, NetworkStatusObserver>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}