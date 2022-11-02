using WpfAppTemplate.Core.Model;
using WpfAppTemplate.Core.Model.Interfaces;
using WpfAppTemplate.Model;
using WpfAppTemplate.Model.Services;
using WpfAppTemplate.View;
using WpfAppTemplate.ViewModel;
using Microsoft.SqlServer.Server;
using SimpleInjector;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using WpfAppTemplate.Core.Model.Config;

namespace WpfAppTemplate;


// https://docs.simpleinjector.org/en/latest/wpfintegration.html
static class Program
{
    static readonly Mutex mutex = new(true, "{E1ABB30F-B977-4865-AAA0-0911089F8B79}");

    [STAThread]
    static void Main()
    {
        Config.Init();
        RollingLogger.EnsureLogFolderExists();

#if RELEASE
        RegisterUnhandledException();
#endif
        RunApplication();
    }


    // https://stackoverflow.com/questions/28275119/how-can-i-check-if-my-program-is-already-running


    private static Container Bootstrap()
    {
        // Create the container as usual.
        var container = new Container();

        // Register your types, for instance:
        container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);
        container.Register<Config>();

        // Register your windows and view models:
        container.Register<ConfirmWindow>();



        // We do this only for main window...
        container.Register<MainWindow>(Lifestyle.Singleton);
        container.Register<MainViewModel>(Lifestyle.Singleton);

        container.Verify();

        return container;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Is not a problem in Release mode!")]
    private static void RegisterUnhandledException()
    {
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
    }

    private static void RunApplication()
    {
        if (mutex.WaitOne(TimeSpan.Zero, true))
        {
            try
            {
                // run the application
                var app = new App();
                app.InitializeComponent();
                var container = Bootstrap();
                var mainWindow = container.GetInstance<MainWindow>();
#if DEBUG
                app.Run(mainWindow);
#else
                try
                {
                    app.Run(mainWindow);
                }
                catch (Exception ex)
                {
                    //Log the exception and exit
                    RollingLogger.Error(ex.Message, ex.StackTrace, ex.Source);
                    CreateMiniDump();
                }
#endif
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        else
        {
            RollingLogger.Info("only one instance at a time");
        }

    }

    // https://brakertech.com/howto-c-generate-dump-file-on-crash/
    public static class MINIDUMP_TYPE
    {
        public const int MiniDumpNormal = 0x00000000;
        public const int MiniDumpWithDataSegs = 0x00000001;
        public const int MiniDumpWithFullMemory = 0x00000002;
        public const int MiniDumpWithHandleData = 0x00000004;
        public const int MiniDumpFilterMemory = 0x00000008;
        public const int MiniDumpScanMemory = 0x00000010;
        public const int MiniDumpWithUnloadedModules = 0x00000020;
        public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
        public const int MiniDumpFilterModulePaths = 0x00000080;
        public const int MiniDumpWithProcessThreadData = 0x00000100;
        public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
        public const int MiniDumpWithoutOptionalData = 0x00000400;
        public const int MiniDumpWithFullMemoryInfo = 0x00000800;
        public const int MiniDumpWithThreadInfo = 0x00001000;
        public const int MiniDumpWithCodeSegs = 0x00002000;
    }

    [DllImport("dbghelp.dll")]
    public static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                int ProcessId,
                                                IntPtr hFile,
                                                int DumpType,
                                                IntPtr ExceptionParam,
                                                IntPtr UserStreamParam,
                                                IntPtr CallackParam);

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        CreateMiniDump();
    }

    private static void CreateMiniDump()
    {
        using FileStream fs = new($"{Core.Model.Config.Pragmas.FolderPath}/UnhandledDump {DateTime.Now:yyyy-MM-dd HHmmss}.dmp", FileMode.Create);
        using System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
        MiniDumpWriteDump(process.Handle,
                          process.Id,
                          fs.SafeFileHandle.DangerousGetHandle(),
                          MINIDUMP_TYPE.MiniDumpNormal,
                          IntPtr.Zero,
                          IntPtr.Zero,
                          IntPtr.Zero);
    }
}