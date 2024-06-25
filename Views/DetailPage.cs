using CSharpMarkupPeopleInSpaceMaui.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;
using LayoutOptions = Microsoft.Maui.Controls.LayoutOptions;

namespace CSharpMarkupPeopleInSpaceMaui.Views;

public class DetailPage : ReactiveContentPage<DetailPageViewModel>
{
    public DetailPage(DetailPageViewModel detailPageViewModel)
    {
        // Set the binding context
        BindingContext = ViewModel = detailPageViewModel;

        // Set up the page title binding
        this.Bind(ViewModel, vm => vm.PageTitle, v => v.Title);

        // Create the main layout
        var scrollView = CreateScrollView();
        
        // Set the content of the page
        Content = scrollView;

        this.WhenActivated(disposables =>
        {
            // Any disposables here
        });
    }

    private static ScrollView CreateScrollView()
    {
        var grid = CreateGrid();
        var scrollView = new ScrollView { Content = grid };
        return scrollView;
    }

    private static Grid CreateGrid()
    {
        var grid = new Grid
        {
            Padding = new Thickness(0)
        };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3000) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        var image = CreateImage();
        grid.Children.Add(image);
        Grid.SetRow(image, 0);
        Grid.SetColumn(image, 0);

        var nameLabel = CreateNameLabel();
        grid.Children.Add(nameLabel);
        Grid.SetRow(nameLabel, 1);
        Grid.SetColumn(nameLabel, 0);

        var webView = CreateWebView();
        grid.Children.Add(webView);
        Grid.SetRow(webView, 2);
        Grid.SetColumn(webView, 0);

        return grid;
    }

    private static Image CreateImage()
    {
        var image = new Image
        {
            HorizontalOptions = LayoutOptions.Center
        };
        image.SetBinding(Image.SourceProperty, "CrewMember.Image");
        return image;
    }

    private static Label CreateNameLabel()
    {
        var nameLabel = new Label
        {
            Padding = new Thickness(10),
            FontSize = 24, // FontSize.Large equivalent
            HorizontalOptions = LayoutOptions.Center
        };
        nameLabel.SetBinding(Label.TextProperty, "CrewMember.Name");
        return nameLabel;
    }

    private static WebView CreateWebView()
    {
        var webView = new WebView
        {
            VerticalOptions = LayoutOptions.Fill
        };
        webView.SetBinding(WebView.SourceProperty, "CrewMember.Wikipedia");
        return webView;
    }
}