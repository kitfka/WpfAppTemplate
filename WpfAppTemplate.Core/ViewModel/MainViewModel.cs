using WpfAppTemplate.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using WpfAppTemplate.Core.Model.Interfaces;

namespace WpfAppTemplate.ViewModel;

public class MainViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; set; }

    public string BindingText { get; set; } = "Hello World!";

    public MainViewModel(INavigationService navigation)
    {
        NavigationService = navigation;
    }
}
