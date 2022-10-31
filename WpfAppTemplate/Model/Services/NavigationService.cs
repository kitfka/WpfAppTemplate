using WpfAppTemplate.Core.Model;
using WpfAppTemplate.Core.Model.Interfaces;
using WpfAppTemplate.View;
using SimpleInjector;
using System;

namespace WpfAppTemplate.Model.Services;


public class NavigationService : INavigationService
{
    private readonly Container container;

    public NavigationService(Container container)
    {
        this.container = container;
    }

    public void ShowView<TView>() where TView : class, IMyWindow
    {
        var view = this.CreateWindow<TView>();

        view.Show();
    }

    public bool? ShowDialog<TView>() where TView : class, IMyWindow
    {
        var view = this.CreateWindow<TView>();

        return view.ShowDialog();
    }

    public TView CreateWindow<TView>() where TView : class, IMyWindow
    {
        return this.container.GetInstance<TView>();
    }

#nullable enable
    public void Confirm(Action? action, string message)
    {
        var view = container.GetInstance<ConfirmWindow>();

        view.Init(action, message);

        view.Show();
    }

    public void Confirm(Action? action, string message, ConfirmWindowOptions options)
    {
        var view = container.GetInstance<ConfirmWindow>();

        view.Init(action, message, options);

        view.Show();
    }
}
