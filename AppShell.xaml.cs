using CSharpMarkupPeopleInSpaceMaui.Views;

namespace CSharpMarkupPeopleInSpaceMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
    }
}