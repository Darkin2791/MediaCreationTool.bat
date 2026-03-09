using System.Windows;
using OfferBuilder.Application.ViewModels;

namespace OfferBuilder.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetService(typeof(MainViewModel)) as MainViewModel;
        Loaded += async (_, _) =>
        {
            if (DataContext is MainViewModel vm)
                await vm.LoadAsync();
        };
    }
}
