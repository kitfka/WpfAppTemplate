
namespace WpfAppTemplate.Core.Model.Interfaces
{
    public interface INavigationService
    {
        bool? ShowDialog<TView>() where TView : class, IMyWindow;
        void ShowView<TView>() where TView : class, IMyWindow;
#nullable enable
        void Confirm(Action? action, string message);

        void Confirm(Action? action, string message, ConfirmWindowOptions options);

        TView CreateWindow<TView>() where TView : class, IMyWindow;
    }
}
