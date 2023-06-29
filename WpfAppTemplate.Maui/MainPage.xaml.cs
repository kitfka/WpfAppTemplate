using WpfAppTemplate.ViewModel;

namespace WpfAppTemplate.Maui;

public partial class MainPage : ContentPage
{
    int count = 0;
    private MainViewModel viewModel;

    public MainPage()
    {
        InitializeComponent();
        viewModel = new MainViewModel(null);

        BindingContext = viewModel;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        viewModel.IncreaseCounter();


        //count++;

        //if (count == 1)
        //    CounterBtn.Text = $"Clicked {count} time";
        //else
        //    CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

