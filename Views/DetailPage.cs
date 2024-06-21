using CSharpMarkupPeopleInSpaceMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using LayoutOptions = Microsoft.Maui.Controls.LayoutOptions;

namespace CSharpMarkupPeopleInSpaceMaui.Views;

public class DetailPage : ReactiveContentPage<DetailPageViewModel>
{
    public DetailPage(IServiceProvider serviceProvider)
    {
        // Set the binding context
        var detailPageViewModel = serviceProvider.GetRequiredService<DetailPageViewModel>();
        BindingContext = detailPageViewModel;
        ViewModel = detailPageViewModel;

        // Set up the page title binding
        this.Bind(ViewModel, vm => vm.PageTitle, v => v.Title);

        // Create the ScrollView
        var scrollView = new ScrollView();

        // Create the Grid
        var grid = new Grid
        {
            Padding = new Thickness(0)
        };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3000) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        // Create and bind the Image
        var image = new Image
        {
            HorizontalOptions = LayoutOptions.Center
        };
        image.SetBinding(Image.SourceProperty, "CrewMember.Image");
        grid.Children.Add(image);
        Grid.SetRow(image, 0);
        Grid.SetColumn(image, 0);

        // Create and bind the Label
        var nameLabel = new Label
        {
            Padding = new Thickness(10),
            FontSize = 24, // FontSize.Large equivalent
            HorizontalOptions = LayoutOptions.Center
        };
        nameLabel.SetBinding(Label.TextProperty, "CrewMember.Name");
        grid.Children.Add(nameLabel);
        Grid.SetRow(nameLabel, 1);
        Grid.SetColumn(nameLabel, 0);

        // Create and bind the WebView
        var webView = new WebView
        {
            VerticalOptions = LayoutOptions.Fill
        };
        webView.SetBinding(WebView.SourceProperty, "CrewMember.Wikipedia");
        grid.Children.Add(webView);
        Grid.SetRow(webView, 2);
        Grid.SetColumn(webView, 0);

        // Add the Grid to the ScrollView
        scrollView.Content = grid;

        // Set the content of the page
        Content = scrollView;

        this.WhenActivated(disposables =>
        {
            // Any disposables here
        });
    }
}