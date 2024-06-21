using ReactiveUI;

namespace CSharpMarkupPeopleInSpaceMaui.Views;

using Microsoft.Maui.Controls;
using ReactiveUI.Maui;
using ViewModels;


public class MainPage : ReactiveContentPage<MainPageViewModel>
{
    public MainPage(IServiceProvider serviceProvider)
    {
        // Set the binding context
        var mainPageViewModel = serviceProvider.GetRequiredService<MainPageViewModel>();
        BindingContext = mainPageViewModel;
        ViewModel = mainPageViewModel;

        // Set up the page title binding
        this.Bind(ViewModel, vm => vm.PageTitle, v => v.Title);

        // Create the RefreshView
        var refreshView = new RefreshView
        {
            Command = ViewModel.LoadCommand,
            CommandParameter = true
        };
        refreshView.SetBinding(RefreshView.IsRefreshingProperty, "IsRefreshing");
        
        // Create the ScrollView and CollectionView
        var scrollView = new ScrollView();
        var collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.None,
            EmptyView = "Please pull to refresh the view",
            Margin = new Thickness(10)
        };
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, "Crew");

        // Define the linear layout
        var itemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
        {
            ItemSpacing = 10
        };
        collectionView.ItemsLayout = itemsLayout;

        // Define the data template
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 5,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.Fill
            };

            var grid = new Grid
            {
                Padding = new Thickness(0)
            };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, new Binding("BindingContext.NavigateToDetailCommand", source: this));
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, ".");
            grid.GestureRecognizers.Add(tapGestureRecognizer);

            var image = new Image
            {
                Aspect = Aspect.AspectFill,
                HeightRequest = 150,
                WidthRequest = 150,
                Margin = new Thickness(0)
            };
            image.SetBinding(Image.SourceProperty, "Image");

            var nameLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 10, 10, 0)
            };
            nameLabel.SetBinding(Label.TextProperty, "Name");

            var agencyLabel = new Label
            {
                FontSize = 16,
                Margin = new Thickness(10, 0, 10, 10)
            };
            agencyLabel.SetBinding(Label.TextProperty, "Agency");

            grid.Children.Add(image);
            grid.Children.Add(nameLabel);
            grid.Children.Add(agencyLabel);

            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 1);
            Grid.SetRow(agencyLabel, 1);
            Grid.SetColumn(agencyLabel, 1);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            Grid.SetRowSpan(image, 2);
            
            frame.Content = grid;

            return frame;
        });

        // Add the CollectionView to the ScrollView and then to the RefreshView
        scrollView.Content = collectionView;
        refreshView.Content = scrollView;

        // Set the content of the page
        Content = refreshView;
        
        this.WhenActivated(disposables =>
        {
        });
    }
    
    protected override void OnAppearing()
    {
        ViewModel?.LoadCommand.Execute(false).Subscribe();
        
        base.OnAppearing();
    }
}