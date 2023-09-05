using WpfAppTemplate.Core.Model.Config;
using WpfAppTemplate.ViewModel;

namespace WpfAppTemplate.Maui.Views;

public partial class SettingsPage : ContentPage
{
    ConfigViewModel viewModel => BindingContext as ConfigViewModel;


    public SettingsPage(ConfigViewModel configViewModel)
	{
		BindingContext = configViewModel;
		InitializeComponent();
	}
}