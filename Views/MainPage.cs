using ReactiveUI;
using ReactiveUI.Maui;
using CSharpMarkupPeopleInSpaceMaui.ViewModels;
using LayoutOptions = Microsoft.Maui.Controls.LayoutOptions;
using CSharpMarkupPeopleInSpaceMaui.HotReload;
using System.Reactive.Disposables;

namespace CSharpMarkupPeopleInSpaceMaui.Views;

public class MainPage : ReactiveContentPage<MainPageViewModel>
{
    private readonly HotReloadHelper _hotReloadHelper;
    
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        BindingContext = ViewModel = mainPageViewModel;
        this.Bind(ViewModel, vm => vm.PageTitle, v => v.Title);

        _hotReloadHelper = new HotReloadHelper(this, Build);
        
        this.WhenActivated(disposables =>
        {
            // Any disposables here
            Disposable.Create(() => _hotReloadHelper.Dispose()).DisposeWith(disposables);

        });

    }

    void Build() => Content =
        CreateRefreshView();

    /*
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Build();

#if DEBUG
        HotReloadService.UpdateApplicationEvent += ReloadUI;
#endif
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

#if DEBUG
        HotReloadService.UpdateApplicationEvent -= ReloadUI;
#endif
    }

    private void ReloadUI(Type[] obj)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Build();
        });
    }*/

    private RefreshView CreateRefreshView()
    {
        var refreshView = new RefreshView
        {
            Command = ViewModel?.LoadCommand,
            CommandParameter = true
        };
        refreshView.SetBinding(RefreshView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
        refreshView.Content = CreateScrollView();

        return refreshView;
    }

    private ScrollView CreateScrollView()
    {
        var scrollView = new ScrollView
        {
            Content = CreateCollectionView()
        };
        return scrollView;
    }

    private CollectionView CreateCollectionView()
    {
        var collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.None,
            EmptyView = "Please pull to refresh the view",
            Margin = new Thickness(10),
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 10 },
            ItemTemplate = CreateItemTemplate()
        };
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, nameof(ViewModel.Crew));

        return collectionView;
    }

    private DataTemplate CreateItemTemplate()
    {
        return new DataTemplate(() =>
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.Blue,
                CornerRadius = 5,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.Fill
            };

            var grid = CreateGrid();
            frame.Content = grid;

            return frame;
        });
    }

    private Grid CreateGrid()
    {
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

        var image = CreateImage();
        var nameLabel = CreateNameLabel();
        var agencyLabel = CreateAgencyLabel();

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

        return grid;
    }

    private static Image CreateImage()
    {
        var image = new Image
        {
            Aspect = Aspect.AspectFill,
            HeightRequest = 150,
            WidthRequest = 150,
            Margin = new Thickness(0)
        };
        image.SetBinding(Image.SourceProperty, "Image");
        return image;
    }

    private static Label CreateNameLabel()
    {
        var nameLabel = new Label
        {
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(10, 10, 10, 0)
        };
        nameLabel.SetBinding(Label.TextProperty, "Name");
        return nameLabel;
    }

    private static Label CreateAgencyLabel()
    {
        var agencyLabel = new Label
        {
            FontSize = 32,
            Margin = new Thickness(10, 0, 10, 10)
        };
        agencyLabel.SetBinding(Label.TextProperty, "Agency");
        return agencyLabel;
    }

    protected override void OnAppearing()
    {
        ViewModel?.LoadCommand.Execute(false).Subscribe();
        base.OnAppearing();
    }
}