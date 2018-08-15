using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using sc_ANN.ViewModels;
using sc_ANN.Views;

namespace sc_ANN
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
